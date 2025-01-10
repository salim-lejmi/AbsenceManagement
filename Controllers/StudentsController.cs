using Absence.Data;
using Absence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Absence.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AbsenceDbContext _context;

        public StudentsController(AbsenceDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var students = await _context.Etudiants
                    .Include(s => s.Classe)
                    .ToListAsync();
                return View(students);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Create()
        {
            try
            {
                ViewData["Classes"] = _context.Classes.ToList();
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Prenom,DateNaissance,CodeClasse,NumInscription,Adresse,Mail,Tel,Password")] T_Etudiant student)
        {
            try
            {
                ModelState.Remove("Classe");
                ModelState.Remove("LignesFicheAbsence");

                if (!ModelState.IsValid)
                {
                    ViewData["Classes"] = await _context.Classes.ToListAsync();
                    return View(student);
                }

                var existingStudent = await _context.Etudiants
                    .FirstOrDefaultAsync(s => s.NumInscription == student.NumInscription);

                if (existingStudent != null)
                {
                    ModelState.AddModelError("NumInscription", "Ce numéro d'enregistrement est déjà utilisé.");
                    ViewData["Classes"] = await _context.Classes.ToListAsync();
                    return View(student);
                }

                _context.Add(student);
                await _context.SaveChangesAsync();

                var user = new T_User
                {
                    Email = student.Mail,
                    Password = student.Password,
                    UserType = "Student",
                    StudentId = student.CodeEtudiant
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Une erreur s'est produite lors de l'enregistrement de l'étudiant.");
                ViewData["Classes"] = await _context.Classes.ToListAsync();
                return View(student);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var student = await _context.Etudiants.FindAsync(id);
                if (student == null) return NotFound();

                ViewData["Classes"] = _context.Classes.ToList();
                return View(student);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodeEtudiant,Nom,Prenom,DateNaissance,CodeClasse,NumInscription,Adresse,Mail,Tel")] T_Etudiant student)
        {
            try
            {
                student.CodeEtudiant = id;
                ModelState.Remove("Classe");
                ModelState.Remove("LignesFicheAbsence");

                if (ModelState.IsValid)
                {
                    var existingStudent = await _context.Etudiants.FindAsync(id);
                    if (existingStudent == null) return NotFound();

                    var duplicateStudent = await _context.Etudiants
                        .FirstOrDefaultAsync(s => s.NumInscription == student.NumInscription && s.CodeEtudiant != id);

                    if (duplicateStudent != null)
                    {
                        ModelState.AddModelError("NumInscription", "Ce numéro d'enregistrement est déjà utilisé.");
                        ViewData["Classes"] = await _context.Classes.ToListAsync();
                        return View(student);
                    }

                    existingStudent.Nom = student.Nom;
                    existingStudent.Prenom = student.Prenom;
                    existingStudent.DateNaissance = student.DateNaissance;
                    existingStudent.CodeClasse = student.CodeClasse;
                    existingStudent.NumInscription = student.NumInscription;
                    existingStudent.Adresse = student.Adresse;
                    existingStudent.Mail = student.Mail;
                    existingStudent.Tel = student.Tel;
                    existingStudent.Password = student.Password;

                    
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Erreur lors de la mise à jour de l'étudiant.");
            }

            ViewData["Classes"] = await _context.Classes.ToListAsync();
            return View(student);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Etudiants
                .Include(s => s.Classe)
                .FirstOrDefaultAsync(s => s.CodeEtudiant == id);

            if (student == null) return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var student = await _context.Etudiants
                    .Include(s => s.Classe)
                    .FirstOrDefaultAsync(s => s.CodeEtudiant == id);

                if (student == null) return NotFound();

                var lignesFiche = await _context.LignesFicheAbsence
                    .Where(lfa => lfa.CodeEtudiant == id)
                    .ToListAsync();
                _context.LignesFicheAbsence.RemoveRange(lignesFiche);

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.StudentId == student.CodeEtudiant);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }

                _context.Etudiants.Remove(student);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

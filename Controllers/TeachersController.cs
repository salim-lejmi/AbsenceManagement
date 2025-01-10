using Absence.Data;
using Absence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Absence.Controllers
{
    public class TeachersController : Controller
    {
        private readonly AbsenceDbContext _context;

        public TeachersController(AbsenceDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _context.Enseignants
                .Include(t => t.Departement)
                .Include(t => t.Grade)
                .ToListAsync();
            return View(teachers);
        }

        public IActionResult Create()
        {
            ViewData["Departments"] = _context.Departements.ToList();
            ViewData["Grades"] = _context.Grades.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Prenom,DateRecrutement,Adresse,Mail,Tel,CodeDepartement,CodeGrade,Password")] T_Enseignant teacher)
        {
            try
            {
                ModelState.Remove("Departement");
                ModelState.Remove("Grade");
                ModelState.Remove("FichesAbsence");

                if (ModelState.IsValid)
                {
                    _context.Add(teacher);
                    await _context.SaveChangesAsync();

                    var user = new T_User
                    {
                        Email = teacher.Mail,
                        Password = teacher.Password,
                        UserType = "Teacher",
                        TeacherId = teacher.CodeEnseignant
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch { }

            ViewData["Departments"] = await _context.Departements.ToListAsync();
            ViewData["Grades"] = await _context.Grades.ToListAsync();
            return View(teacher);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Enseignants.FindAsync(id);
            if (teacher == null) return NotFound();

            ViewData["Departments"] = _context.Departements.ToList();
            ViewData["Grades"] = _context.Grades.ToList();
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodeEnseignant,Nom,Prenom,DateRecrutement,Adresse,Mail,Tel,CodeDepartement,CodeGrade,Password")] T_Enseignant teacher)
        {
            try
            {
                teacher.CodeEnseignant = id;
                ModelState.Remove("Departement");
                ModelState.Remove("Grade");
                ModelState.Remove("FichesAbsence");

                if (ModelState.IsValid)
                {
                    var existingTeacher = await _context.Enseignants.FindAsync(id);
                    if (existingTeacher != null)
                    {
                        existingTeacher.Nom = teacher.Nom;
                        existingTeacher.Prenom = teacher.Prenom;
                        existingTeacher.DateRecrutement = teacher.DateRecrutement;
                        existingTeacher.Adresse = teacher.Adresse;
                        existingTeacher.Mail = teacher.Mail;
                        existingTeacher.Tel = teacher.Tel;
                        existingTeacher.CodeDepartement = teacher.CodeDepartement;
                        existingTeacher.CodeGrade = teacher.CodeGrade;
                        existingTeacher.Password = teacher.Password;

                        var user = await _context.Users
    .FirstOrDefaultAsync(u => u.Email == teacher.Mail);
                        if (user != null)
                        {
                            user.Password = teacher.Password;
                            await _context.SaveChangesAsync();
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch { }

            ViewData["Departments"] = await _context.Departements.ToListAsync();
            ViewData["Grades"] = await _context.Grades.ToListAsync();
            return View(teacher);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Enseignants
                .Include(t => t.Departement)
                .Include(t => t.Grade)
                .FirstOrDefaultAsync(t => t.CodeEnseignant == id);

            if (teacher == null) return NotFound();

            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var teacher = await _context.Enseignants
                    .Include(t => t.FichesAbsence)
                    .FirstOrDefaultAsync(t => t.CodeEnseignant == id);

                if (teacher != null)
                {
                    _context.Enseignants.Remove(teacher);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch { }

            return RedirectToAction(nameof(Index));
        }
    }
}

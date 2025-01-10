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
        public async Task<IActionResult> Create([Bind("Nom,Prenom,DateRecrutement,Adresse,Mail,Tel,CodeDepartement,CodeGrade")] T_Enseignant teacher)
        {
            Console.WriteLine("\n=== Starting Teacher Create Process ===");

            try
            {
                ModelState.Remove("Departement");
                ModelState.Remove("Grade");
                ModelState.Remove("FichesAbsence");

                Console.WriteLine("Incoming Teacher Data:");
                foreach (var prop in typeof(T_Enseignant).GetProperties())
                {
                    Console.WriteLine($"{prop.Name}: {prop.GetValue(teacher)}");
                }

                Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState Errors:");
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine($"- {error.ErrorMessage}");
                        }
                    }

                    ViewData["Departments"] = await _context.Departements.ToListAsync();
                    ViewData["Grades"] = await _context.Grades.ToListAsync();
                    return View(teacher);
                }

                if (!string.IsNullOrEmpty(teacher.Mail))
                {
                    var existingTeacher = await _context.Enseignants
                        .FirstOrDefaultAsync(t => t.Mail == teacher.Mail);

                    if (existingTeacher != null)
                    {
                        Console.WriteLine($"Teacher with email {teacher.Mail} already exists");
                        ModelState.AddModelError("Mail", "This email is already in use.");
                        ViewData["Departments"] = await _context.Departements.ToListAsync();
                        ViewData["Grades"] = await _context.Grades.ToListAsync();
                        return View(teacher);
                    }
                }

                Console.WriteLine("Adding teacher to context");
                _context.Add(teacher);

                Console.WriteLine("Saving changes to database");
                await _context.SaveChangesAsync();

                Console.WriteLine("Creating user account for the teacher");
                var user = new T_User
                {
                    Email = teacher.Mail,
                    Password = teacher.Tel, 
                    UserType = "Teacher",
                    TeacherId = teacher.CodeEnseignant 
                };

                Console.WriteLine("Adding user account to context");
                _context.Users.Add(user);

                Console.WriteLine("Saving changes to database for user account");
                await _context.SaveChangesAsync();

                Console.WriteLine("Teacher and user account created successfully. Redirecting to Index");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", "An error occurred while saving the teacher.");
                ViewData["Departments"] = await _context.Departements.ToListAsync();
                ViewData["Grades"] = await _context.Grades.ToListAsync();
                return View(teacher);
            }
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
        public async Task<IActionResult> Edit(int id, [Bind("CodeEnseignant,Nom,Prenom,DateRecrutement,Adresse,Mail,Tel,CodeDepartement,CodeGrade")] T_Enseignant teacher)
        {
            try
            {
                Console.WriteLine($"Teacher Edit - Incoming Data for ID: {id}");
                teacher.CodeEnseignant = id;

                ModelState.Remove("Departement");
                ModelState.Remove("Grade");
                ModelState.Remove("FichesAbsence");

                if (ModelState.IsValid)
                {
                    try
                    {
                        var existingTeacher = await _context.Enseignants.FindAsync(id);
                        if (existingTeacher == null)
                        {
                            return NotFound();
                        }

                        if (!string.IsNullOrEmpty(teacher.Mail))
                        {
                            var duplicateTeacher = await _context.Enseignants
                                .FirstOrDefaultAsync(t => t.Mail == teacher.Mail && t.CodeEnseignant != id);

                            if (duplicateTeacher != null)
                            {
                                ModelState.AddModelError("Mail", "This email is already in use.");
                                ViewData["Departments"] = await _context.Departements.ToListAsync();
                                ViewData["Grades"] = await _context.Grades.ToListAsync();
                                return View(teacher);
                            }
                        }

                        existingTeacher.Nom = teacher.Nom;
                        existingTeacher.Prenom = teacher.Prenom;
                        existingTeacher.DateRecrutement = teacher.DateRecrutement;
                        existingTeacher.Adresse = teacher.Adresse;
                        existingTeacher.Mail = teacher.Mail;
                        existingTeacher.Tel = teacher.Tel;
                        existingTeacher.CodeDepartement = teacher.CodeDepartement;
                        existingTeacher.CodeGrade = teacher.CodeGrade;

                        await _context.SaveChangesAsync();
                        Console.WriteLine("Teacher Edit - Updated successfully");
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        Console.WriteLine($"Teacher Edit - Concurrency error: {ex.Message}");
                        if (!_context.Enseignants.Any(e => e.CodeEnseignant == id))
                        {
                            return NotFound();
                        }
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Teacher Edit - Exception: {ex.Message}");
                ModelState.AddModelError("", "Error updating teacher: " + ex.Message);
            }

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

                if (teacher == null)
                    return NotFound();

                // Handle associated FichesAbsence
                if (teacher.FichesAbsence != null && teacher.FichesAbsence.Any())
                {
                    foreach (var fiche in teacher.FichesAbsence)
                    {
                        // Remove associated FicheAbsenceSeances
                        var ficheSeances = await _context.FicheAbsenceSeances
                            .Where(fas => fas.CodeFicheAbsence == fiche.CodeFicheAbsence)
                            .ToListAsync();
                        _context.FicheAbsenceSeances.RemoveRange(ficheSeances);

                        // Remove associated LigneFicheAbsence
                        var lignesFiche = await _context.LignesFicheAbsence
                            .Where(lfa => lfa.CodeFicheAbsence == fiche.CodeFicheAbsence)
                            .ToListAsync();
                        _context.LignesFicheAbsence.RemoveRange(lignesFiche);

                        // Remove the FicheAbsence
                        _context.FichesAbsence.Remove(fiche);
                    }
                }

                // Remove associated user account if exists
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == teacher.Mail);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }

                // Finally, remove the teacher
                _context.Enseignants.Remove(teacher);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // Redirect to Index if any error occurs
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
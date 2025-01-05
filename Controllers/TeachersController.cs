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
            try
            {
                // Debug: Log the incoming model data
                Console.WriteLine("Teacher Create - Incoming Data:");
                Console.WriteLine($"Nom: {teacher.Nom}");
                Console.WriteLine($"Prenom: {teacher.Prenom}");
                Console.WriteLine($"DateRecrutement: {teacher.DateRecrutement}");
                Console.WriteLine($"Adresse: {teacher.Adresse}");
                Console.WriteLine($"Mail: {teacher.Mail}");
                Console.WriteLine($"Tel: {teacher.Tel}");
                Console.WriteLine($"CodeDepartement: {teacher.CodeDepartement}");
                Console.WriteLine($"CodeGrade: {teacher.CodeGrade}");

                if (ModelState.IsValid)
                {
                    // Debug: Log that the model state is valid
                    Console.WriteLine("Teacher Create - ModelState is valid.");

                    _context.Enseignants.Add(teacher);
                    await _context.SaveChangesAsync();

                    // Debug: Log success
                    Console.WriteLine("Teacher Create - Teacher saved successfully.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Debug: Log validation errors
                    Console.WriteLine("Teacher Create - ModelState is invalid. Errors:");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Debug: Log the exception details
                Console.WriteLine("Teacher Create - Exception occurred:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                ModelState.AddModelError("", "Unable to save changes. " + ex.Message);
            }

            // Debug: Log that we're returning to the form
            Console.WriteLine("Teacher Create - Returning view with the provided teacher data.");
            ViewData["Departments"] = _context.Departements.ToList();
            ViewData["Grades"] = _context.Grades.ToList();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodeEnseignant,Nom,Prenom,DateRecrutement,Adresse,Mail,Tel,CodeDepartement,CodeGrade")] T_Enseignant teacher)
        {
            try
            {
                // Debug: Log the incoming model data
                Console.WriteLine("Teacher Edit - Incoming Data:");
                Console.WriteLine($"CodeEnseignant: {teacher.CodeEnseignant}");
                Console.WriteLine($"Nom: {teacher.Nom}");
                Console.WriteLine($"Prenom: {teacher.Prenom}");
                Console.WriteLine($"DateRecrutement: {teacher.DateRecrutement}");
                Console.WriteLine($"Adresse: {teacher.Adresse}");
                Console.WriteLine($"Mail: {teacher.Mail}");
                Console.WriteLine($"Tel: {teacher.Tel}");
                Console.WriteLine($"CodeDepartement: {teacher.CodeDepartement}");
                Console.WriteLine($"CodeGrade: {teacher.CodeGrade}");

                if (id != teacher.CodeEnseignant)
                {
                    // Debug: Log ID mismatch
                    Console.WriteLine($"Teacher Edit - ID mismatch: id = {id}, teacher.CodeEnseignant = {teacher.CodeEnseignant}");
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        // Debug: Log that the model state is valid
                        Console.WriteLine("Teacher Edit - ModelState is valid.");

                        var existingTeacher = await _context.Enseignants.FindAsync(id);
                        if (existingTeacher == null)
                        {
                            Console.WriteLine($"Teacher Edit - Teacher with ID {id} not found.");
                            return NotFound();
                        }

                        // Update the existing teacher's properties
                        _context.Entry(existingTeacher).CurrentValues.SetValues(teacher);
                        await _context.SaveChangesAsync();

                        // Debug: Log success
                        Console.WriteLine("Teacher Edit - Teacher updated successfully.");
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        // Debug: Log concurrency exception
                        Console.WriteLine("Teacher Edit - Concurrency exception occurred:");
                        Console.WriteLine(ex.Message);
                        if (!_context.Enseignants.Any(e => e.CodeEnseignant == id))
                        {
                            Console.WriteLine($"Teacher Edit - Teacher with ID {id} no longer exists.");
                            return NotFound();
                        }
                        throw;
                    }
                }
                else
                {
                    // Debug: Log validation errors
                    Console.WriteLine("Teacher Edit - ModelState is invalid. Errors:");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Debug: Log the exception details
                Console.WriteLine("Teacher Edit - Exception occurred:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                ModelState.AddModelError("", "Unable to save changes. " + ex.Message);
            }

            // Debug: Log that we're returning to the form
            Console.WriteLine("Teacher Edit - Returning view with the provided teacher data.");
            ViewData["Departments"] = _context.Departements.ToList();
            ViewData["Grades"] = _context.Grades.ToList();
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
            var teacher = await _context.Enseignants.FindAsync(id);
            _context.Enseignants.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
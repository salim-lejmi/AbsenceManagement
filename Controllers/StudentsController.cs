using Absence.Data;
using Absence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
                Debug.WriteLine("Fetching students from database...");
                var students = await _context.Etudiants
                    .Include(s => s.Classe)
                    .ToListAsync();
                Debug.WriteLine($"Found {students.Count} students");
                return View(students);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in Index: {ex.Message}");
                throw;
            }
        }

        public IActionResult Create()
        {
            try
            {
                Debug.WriteLine("Loading classes for Create view...");
                var classes = _context.Classes.ToList();
                Debug.WriteLine($"Found {classes.Count} classes");
                ViewData["Classes"] = classes;
                return View();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in Create GET: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Prenom,DateNaissance,CodeClasse,NumInscription,Adresse,Mail,Tel")] T_Etudiant student)
        {
            try
            {
                // Debug: Log the incoming model data
                Console.WriteLine("Student Create - Incoming Data:");
                Console.WriteLine($"Nom: {student.Nom}");
                Console.WriteLine($"Prenom: {student.Prenom}");
                Console.WriteLine($"DateNaissance: {student.DateNaissance}");
                Console.WriteLine($"CodeClasse: {student.CodeClasse}");
                Console.WriteLine($"NumInscription: {student.NumInscription}");
                Console.WriteLine($"Adresse: {student.Adresse}");
                Console.WriteLine($"Mail: {student.Mail}");
                Console.WriteLine($"Tel: {student.Tel}");

                if (ModelState.IsValid)
                {
                    // Debug: Log that the model state is valid
                    Console.WriteLine("Student Create - ModelState is valid.");

                    _context.Etudiants.Add(student);
                    await _context.SaveChangesAsync();

                    // Debug: Log success
                    Console.WriteLine("Student Create - Student saved successfully.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Debug: Log validation errors
                    Console.WriteLine("Student Create - ModelState is invalid. Errors:");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Debug: Log the exception details
                Console.WriteLine("Student Create - Exception occurred:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                ModelState.AddModelError("", "Unable to save changes. " + ex.Message);
            }

            // Debug: Log that we're returning to the form
            Console.WriteLine("Student Create - Returning view with the provided student data.");
            ViewData["Classes"] = _context.Classes.ToList();
            return View(student);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            Debug.WriteLine($"Editing student with ID: {id}");

            if (id == null)
            {
                Debug.WriteLine("Edit failed: ID was null");
                return NotFound();
            }

            try
            {
                var student = await _context.Etudiants.FindAsync(id);
                if (student == null)
                {
                    Debug.WriteLine("Edit failed: Student not found");
                    return NotFound();
                }

                ViewData["Classes"] = _context.Classes.ToList();
                return View(student);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in Edit GET: {ex.Message}");
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodeEtudiant,Nom,Prenom,DateNaissance,CodeClasse,NumInscription,Adresse,Mail,Tel")] T_Etudiant student)
        {
            try
            {
                // Debug: Log the incoming model data
                Console.WriteLine("Student Edit - Incoming Data:");
                Console.WriteLine($"CodeEtudiant: {student.CodeEtudiant}");
                Console.WriteLine($"Nom: {student.Nom}");
                Console.WriteLine($"Prenom: {student.Prenom}");
                Console.WriteLine($"DateNaissance: {student.DateNaissance}");
                Console.WriteLine($"CodeClasse: {student.CodeClasse}");
                Console.WriteLine($"NumInscription: {student.NumInscription}");
                Console.WriteLine($"Adresse: {student.Adresse}");
                Console.WriteLine($"Mail: {student.Mail}");
                Console.WriteLine($"Tel: {student.Tel}");

                if (id != student.CodeEtudiant)
                {
                    // Debug: Log ID mismatch
                    Console.WriteLine($"Student Edit - ID mismatch: id = {id}, student.CodeEtudiant = {student.CodeEtudiant}");
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        // Debug: Log that the model state is valid
                        Console.WriteLine("Student Edit - ModelState is valid.");

                        var existingStudent = await _context.Etudiants.FindAsync(id);
                        if (existingStudent == null)
                        {
                            Console.WriteLine($"Student Edit - Student with ID {id} not found.");
                            return NotFound();
                        }

                        // Update the existing student's properties
                        _context.Entry(existingStudent).CurrentValues.SetValues(student);
                        await _context.SaveChangesAsync();

                        // Debug: Log success
                        Console.WriteLine("Student Edit - Student updated successfully.");
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        // Debug: Log concurrency exception
                        Console.WriteLine("Student Edit - Concurrency exception occurred:");
                        Console.WriteLine(ex.Message);
                        if (!_context.Etudiants.Any(e => e.CodeEtudiant == id))
                        {
                            Console.WriteLine($"Student Edit - Student with ID {id} no longer exists.");
                            return NotFound();
                        }
                        throw;
                    }
                }
                else
                {
                    // Debug: Log validation errors
                    Console.WriteLine("Student Edit - ModelState is invalid. Errors:");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Debug: Log the exception details
                Console.WriteLine("Student Edit - Exception occurred:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                ModelState.AddModelError("", "Unable to save changes. " + ex.Message);
            }

            // Debug: Log that we're returning to the form
            Console.WriteLine("Student Edit - Returning view with the provided student data.");
            ViewData["Classes"] = _context.Classes.ToList();
            return View(student);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Debug.WriteLine($"Attempting to delete student ID: {id}");

            try
            {
                var student = await _context.Etudiants.FindAsync(id);
                if (student != null)
                {
                    _context.Etudiants.Remove(student);
                    await _context.SaveChangesAsync();
                    Debug.WriteLine("Student deleted successfully");
                }
                else
                {
                    Debug.WriteLine("Delete failed: Student not found");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting student: {ex.Message}");
                throw;
            }
        }
    }
}
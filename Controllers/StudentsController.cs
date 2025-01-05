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
            Console.WriteLine("\n=== Starting Student Create Process ===");

            try
            {
                // Remove validation for virtual properties
                ModelState.Remove("Classe");
                ModelState.Remove("LignesFicheAbsence");

                // Log all incoming data
                Console.WriteLine("Incoming Student Data:");
                foreach (var prop in typeof(T_Etudiant).GetProperties())
                {
                    Console.WriteLine($"{prop.Name}: {prop.GetValue(student)}");
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

                    ViewData["Classes"] = await _context.Classes.ToListAsync();
                    return View(student);
                }

                // Check if student with same NumInscription exists
                var existingStudent = await _context.Etudiants
                    .FirstOrDefaultAsync(s => s.NumInscription == student.NumInscription);

                if (existingStudent != null)
                {
                    Console.WriteLine($"Student with NumInscription {student.NumInscription} already exists");
                    ModelState.AddModelError("NumInscription", "This registration number is already in use.");
                    ViewData["Classes"] = await _context.Classes.ToListAsync();
                    return View(student);
                }

                Console.WriteLine("Adding student to context");
                _context.Add(student);

                Console.WriteLine("Saving changes to database");
                await _context.SaveChangesAsync();

                Console.WriteLine("Student created successfully. Redirecting to Index");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", "An error occurred while saving the student.");
                ViewData["Classes"] = await _context.Classes.ToListAsync();
                return View(student);
            }
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
                Console.WriteLine($"Student Edit - Incoming Data for ID: {id}");
                student.CodeEtudiant = id;

                // Remove validation for virtual properties
                ModelState.Remove("Classe");
                ModelState.Remove("LignesFicheAbsence");

                if (ModelState.IsValid)
                {
                    try
                    {
                        var existingStudent = await _context.Etudiants.FindAsync(id);
                        if (existingStudent == null)
                        {
                            return NotFound();
                        }

                        // Check if NumInscription is already in use by another student
                        var duplicateStudent = await _context.Etudiants
                            .FirstOrDefaultAsync(s => s.NumInscription == student.NumInscription && s.CodeEtudiant != id);

                        if (duplicateStudent != null)
                        {
                            ModelState.AddModelError("NumInscription", "This registration number is already in use.");
                            ViewData["Classes"] = await _context.Classes.ToListAsync();
                            return View(student);
                        }

                        // Update existing student properties
                        existingStudent.Nom = student.Nom;
                        existingStudent.Prenom = student.Prenom;
                        existingStudent.DateNaissance = student.DateNaissance;
                        existingStudent.CodeClasse = student.CodeClasse;
                        existingStudent.NumInscription = student.NumInscription;
                        existingStudent.Adresse = student.Adresse;
                        existingStudent.Mail = student.Mail;
                        existingStudent.Tel = student.Tel;

                        await _context.SaveChangesAsync();
                        Console.WriteLine("Student Edit - Updated successfully");
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        Console.WriteLine($"Student Edit - Concurrency error: {ex.Message}");
                        if (!_context.Etudiants.Any(e => e.CodeEtudiant == id))
                        {
                            return NotFound();
                        }
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Student Edit - Exception: {ex.Message}");
                ModelState.AddModelError("", "Error updating student: " + ex.Message);
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
            var student = await _context.Etudiants.FindAsync(id);
            _context.Etudiants.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
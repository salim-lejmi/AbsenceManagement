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
                    Debug.WriteLine("Récupérer les étudiants de la base de données...");
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
                    Debug.WriteLine("Chargement des classes pour la création d'une vue...");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Prenom,DateNaissance,CodeClasse,NumInscription,Adresse,Mail,Tel")] T_Etudiant student)
        {
            Console.WriteLine("\n=== Démarrer le processus de création d'étudiants ===");

            try
            {
                ModelState.Remove("Classe");
                ModelState.Remove("LignesFicheAbsence");

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

                Console.WriteLine("Creating user account for the student");
                var user = new T_User
                {
                    Email = student.Mail,
                    Password = student.NumInscription,
                    UserType = "Student",
                    StudentId = student.CodeEtudiant 
                };

                Console.WriteLine("Adding user account to context");
                _context.Users.Add(user);

                Console.WriteLine("Saving changes to database for user account");
                await _context.SaveChangesAsync();

                Console.WriteLine("Student and user account created successfully. Redirecting to Index");
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
                Debug.WriteLine($"Modification d'un étudiant avec ID: {id}");

                if (id == null)
                {
                    Debug.WriteLine("Échec de la modification : l'ID était nul");
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

                        var duplicateStudent = await _context.Etudiants
                            .FirstOrDefaultAsync(s => s.NumInscription == student.NumInscription && s.CodeEtudiant != id);

                        if (duplicateStudent != null)
                        {
                            ModelState.AddModelError("NumInscription", "This registration number is already in use.");
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
            try
            {
                var student = await _context.Etudiants
                    .Include(s => s.Classe)
                    .FirstOrDefaultAsync(s => s.CodeEtudiant == id);

                if (student == null)
                    return NotFound();

                // Remove associated LigneFicheAbsence records
                var lignesFiche = await _context.LignesFicheAbsence
                    .Where(lfa => lfa.CodeEtudiant == id)
                    .ToListAsync();
                _context.LignesFicheAbsence.RemoveRange(lignesFiche);

                // Remove associated user account if exists
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.StudentId == student.CodeEtudiant);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }

                // Finally, remove the student
                _context.Etudiants.Remove(student);
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
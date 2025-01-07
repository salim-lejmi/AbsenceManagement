using Absence.Data;
using Absence.Models;
using Absence.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Absence.Controllers
{
    public class AbsencesController : Controller
    {
        private readonly AbsenceDbContext _context;

        public AbsencesController(AbsenceDbContext context)
        {
            _context = context;
        }

        // GET: MarkAbsence
        public async Task<IActionResult> MarkAbsence()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (teacherId == null)
            {
                return Unauthorized();
            }

            var viewModel = new MarkAbsenceViewModel
            {
                Classes = await _context.Classes.Include(c => c.Etudiants).ToListAsync(),
                Matieres = await _context.Matieres.ToListAsync(), // Changed from Seances to Matieres
                Date = DateTime.Now
            };

            return View(viewModel);
        }

        // POST: MarkAbsence
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAbsence([Bind("Date,SelectedClassId,SelectedSubjectId,Absences")] MarkAbsenceViewModel model)
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (teacherId == null)
            {
                return Unauthorized();
            }

            try
            {
                // Remove validation for navigation properties
                ModelState.Remove("Classes");
                ModelState.Remove("Matieres");

                // Validate that required fields are selected
                if (model.SelectedClassId == 0)
                {
                    ModelState.AddModelError("SelectedClassId", "Please select a class");
                }
                if (model.SelectedSubjectId == 0)
                {
                    ModelState.AddModelError("SelectedSubjectId", "Please select a subject");
                }

                if (!ModelState.IsValid)
                {
                    // Reload the required data
                    model.Classes = await _context.Classes.Include(c => c.Etudiants).ToListAsync();
                    model.Matieres = await _context.Matieres.ToListAsync();
                    return View(model);
                }

                var ficheAbsence = new T_FicheAbsence
                {
                    DateJour = model.Date,
                    CodeClasse = model.SelectedClassId,
                    CodeMatiere = model.SelectedSubjectId,
                    CodeEnseignant = teacherId.Value
                };

                _context.FichesAbsence.Add(ficheAbsence);
                await _context.SaveChangesAsync();

                // Process absences only if there are any
                if (model.Absences != null && model.Absences.Any())
                {
                    foreach (var absence in model.Absences.Where(a => a.IsAbsent))
                    {
                        var ligneFicheAbsence = new T_LigneFicheAbsence
                        {
                            CodeFicheAbsence = ficheAbsence.CodeFicheAbsence,
                            CodeEtudiant = absence.StudentId
                        };

                        _context.LignesFicheAbsence.Add(ligneFicheAbsence);
                    }

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(MarkAbsence));
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while saving the absence records.");

                // Reload the required data
                model.Classes = await _context.Classes.Include(c => c.Etudiants).ToListAsync();
                model.Matieres = await _context.Matieres.ToListAsync();
                return View(model);
            }
        }




        // GET: Student Absences
        public async Task<IActionResult> StudentAbsences()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                return Unauthorized(); // Students only
            }

            var absences = await _context.LignesFicheAbsence
                .Include(lfa => lfa.FicheAbsence)
                .ThenInclude(fa => fa.Matiere)
                .Include(lfa => lfa.FicheAbsence.Enseignant)
                .Where(lfa => lfa.CodeEtudiant == studentId.Value)
                .ToListAsync();

            var viewModel = absences.Select(a => new AbsenceDetailsViewModel
            {
                Date = a.FicheAbsence.DateJour,
                SeanceName = a.FicheAbsence.FichesAbsenceSeances.FirstOrDefault()?.Seance.NomSeance,
                TeacherName = $"{a.FicheAbsence.Enseignant.Nom} {a.FicheAbsence.Enseignant.Prenom}"
            }).ToList();

            return View(viewModel);
        }

        // Task 5: Absence Report by Date
        public async Task<IActionResult> AbsenceReport(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return View(new List<StudentAbsenceReportViewModel>());
            }

            var absences = await _context.LignesFicheAbsence
                .Include(lfa => lfa.Etudiant)
                .Include(lfa => lfa.FicheAbsence)
                .Where(lfa => lfa.FicheAbsence.DateJour >= startDate && lfa.FicheAbsence.DateJour <= endDate)
                .ToListAsync();

            var viewModel = absences
                .GroupBy(a => a.Etudiant)
                .Select(group => new StudentAbsenceReportViewModel
                {
                    StudentId = group.Key.CodeEtudiant,
                    StudentName = $"{group.Key.Nom} {group.Key.Prenom}",
                    AbsenceCount = group.Count()
                })
                .ToList();

            return View(viewModel);
        }

        // Task 6: Absence Details by Subject
        public async Task<IActionResult> AbsenceDetails(int studentId, int subjectId)
        {
            var absences = await _context.LignesFicheAbsence
                .Include(lfa => lfa.Etudiant)
                .Include(lfa => lfa.FicheAbsence)
                .ThenInclude(fa => fa.Matiere)
                .Include(lfa => lfa.FicheAbsence.Enseignant)
                .Where(lfa => lfa.CodeEtudiant == studentId && lfa.FicheAbsence.CodeMatiere == subjectId)
                .ToListAsync();

            var viewModel = absences.Select(a => new AbsenceDetailsViewModel
            {
                Date = a.FicheAbsence.DateJour,
                SeanceName = a.FicheAbsence.FichesAbsenceSeances.FirstOrDefault()?.Seance.NomSeance,
                TeacherName = $"{a.FicheAbsence.Enseignant.Nom} {a.FicheAbsence.Enseignant.Prenom}"
            }).ToList();

            return View(viewModel);
        }
    }

}
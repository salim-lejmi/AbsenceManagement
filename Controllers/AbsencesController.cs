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

        public async Task<IActionResult> MarkAbsence()
        {
            var viewModel = new MarkAbsenceViewModel
            {
                Classes = await _context.Classes.Include(c => c.Etudiants).ToListAsync(),
                Seances = await _context.Seances.ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAbsence(MarkAbsenceViewModel model)
        {
            try
            {
                // Remove validation for virtual properties
                ModelState.Remove("Classes");
                ModelState.Remove("Seances");

                // Clear existing errors
                foreach (var key in ModelState.Keys)
                {
                    ModelState[key].Errors.Clear();
                }

                if (model.Absences == null)
                {
                    model.Classes = await _context.Classes.Include(c => c.Etudiants).ToListAsync();
                    model.Seances = await _context.Seances.ToListAsync();
                }

                if (!ModelState.IsValid)
                {
                    model.Classes = await _context.Classes.Include(c => c.Etudiants).ToListAsync();
                    model.Seances = await _context.Seances.ToListAsync();
                    return View(model);
                }

                // Adding absence records
                foreach (var absence in model.Absences)
                {
                    if (absence.IsAbsent)
                    {
                        var ligneFicheAbsence = new T_LigneFicheAbsence
                        {
                            CodeFicheAbsence = absence.FicheAbsenceId,
                            CodeEtudiant = absence.StudentId
                        };
                        _context.LignesFicheAbsence.Add(ligneFicheAbsence);
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MarkAbsence));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the absence records.");
                model.Classes = await _context.Classes.Include(c => c.Etudiants).ToListAsync();
                model.Seances = await _context.Seances.ToListAsync();
                return View(model);
            }
        }



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

        public async Task<IActionResult> AbsenceDetails(int studentId, int subjectId)
        {
            var absences = await _context.LignesFicheAbsence
                .Include(lfa => lfa.Etudiant)
                .Include(lfa => lfa.FicheAbsence)
                .ThenInclude(fa => fa.Matiere)
                .Where(lfa => lfa.Etudiant.CodeEtudiant == studentId && lfa.FicheAbsence.CodeMatiere == subjectId)
                .ToListAsync();

            var viewModel = absences.Select(a => new AbsenceDetailsViewModel
            {
                Date = a.FicheAbsence.DateJour,
                SeanceName = a.FicheAbsence.FichesAbsenceSeances.FirstOrDefault()?.Seance.NomSeance
            }).ToList();

            return View(viewModel);
        }


    }
}
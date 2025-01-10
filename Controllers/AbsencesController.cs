﻿using Absence.ViewModels;
using Absence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Absence.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
        

            var classes = await _context.Classes.Include(c => c.Etudiants).ToListAsync();
            var students = classes.SelectMany(c => c.Etudiants).ToList();

            var viewModel = new MarkAbsenceViewModel
            {
                Classes = classes,
                Matieres = await _context.Matieres.ToListAsync(),
                Seances = await _context.Seances.ToListAsync(), 
                Date = DateTime.Now,
                Absences = students.Select(s => new StudentAbsence
                {
                    StudentId = s.CodeEtudiant,
                    IsAbsent = false
                }).ToList()
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAbsence(MarkAbsenceViewModel model)
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
       

            if (model.SelectedClassId == 0 || model.SelectedSubjectId == 0 || model.SelectedSeanceId == 0)
            {
                ModelState.AddModelError("", "Veuillez sélectionner une classe, une matiere et une séance.");
                model.Classes = await _context.Classes.Include(c => c.Etudiants).ToListAsync();
                model.Matieres = await _context.Matieres.ToListAsync();
                model.Seances = await _context.Seances.ToListAsync();
                return View(model);
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var ficheAbsence = new T_FicheAbsence
                    {
                        DateJour = model.Date,
                        CodeClasse = model.SelectedClassId,
                        CodeMatiere = model.SelectedSubjectId,
                        CodeEnseignant = teacherId.Value 
                    };

                    _context.FichesAbsence.Add(ficheAbsence);
                    await _context.SaveChangesAsync();

                    var ficheAbsenceSeance = new T_FicheAbsenceSeance
                    {
                        CodeFicheAbsence = ficheAbsence.CodeFicheAbsence, 
                        CodeSeance = model.SelectedSeanceId
                    };

                    _context.FicheAbsenceSeances.Add(ficheAbsenceSeance);
                    await _context.SaveChangesAsync();

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

                    await transaction.CommitAsync();

                    TempData["Success"] = "Les absences ont été enregistrées avec succès.";
                    return RedirectToAction(nameof(MarkAbsence));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    ModelState.AddModelError("", "Une erreur s'est produite lors de l'enregistrement des absences. Veuillez réessayer.");

                    model.Classes = await _context.Classes.Include(c => c.Etudiants).ToListAsync();
                    model.Matieres = await _context.Matieres.ToListAsync();
                    model.Seances = await _context.Seances.ToListAsync();

                    return View(model);
                }
            }
        }

        public async Task<IActionResult> StudentAbsences()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                return Unauthorized();
            }

            var absences = await _context.LignesFicheAbsence
                .Include(lfa => lfa.FicheAbsence)
                    .ThenInclude(fa => fa.Matiere)
                .Include(lfa => lfa.FicheAbsence)
                    .ThenInclude(fa => fa.Enseignant)
                .Include(lfa => lfa.FicheAbsence)
                    .ThenInclude(fa => fa.FichesAbsenceSeances)
                        .ThenInclude(fas => fas.Seance)
                .Where(lfa => lfa.CodeEtudiant == studentId.Value)
                .OrderByDescending(lfa => lfa.FicheAbsence.DateJour)
                .ToListAsync();

            var viewModel = absences.Select(a => new AbsenceDetailsViewModel
            {
                Date = a.FicheAbsence.DateJour,
                SeanceName = a.FicheAbsence.FichesAbsenceSeances
                    .FirstOrDefault()?.Seance?.NomSeance ?? "N/A",
                TeacherName = $"{a.FicheAbsence.Enseignant.Nom} {a.FicheAbsence.Enseignant.Prenom}",
                SubjectName = a.FicheAbsence.Matiere.NomMatiere
            }).ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> AbsenceReport(DateTime? startDate, DateTime? endDate, int? classeId, int? studentId)
        {
       

            var query = _context.LignesFicheAbsence
                .Include(lfa => lfa.Etudiant)
                    .ThenInclude(e => e.Classe)
                .Include(lfa => lfa.FicheAbsence)
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(lfa => lfa.FicheAbsence.DateJour >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                var adjustedEndDate = endDate.Value.AddDays(1).AddSeconds(-1);
                query = query.Where(lfa => lfa.FicheAbsence.DateJour <= adjustedEndDate);
            }

            if (classeId.HasValue)
            {
                query = query.Where(lfa => lfa.Etudiant.CodeClasse == classeId.Value);
            }

            if (studentId.HasValue)
            {
                query = query.Where(lfa => lfa.CodeEtudiant == studentId.Value);
            }

            var absences = await query.ToListAsync();

            var viewModel = absences
                .GroupBy(a => new { a.Etudiant.CodeEtudiant, a.Etudiant.Nom, a.Etudiant.Prenom, a.Etudiant.Classe.NomClasse })
                .Select(group => new StudentAbsenceReportViewModel
                {
                    StudentId = group.Key.CodeEtudiant,
                    StudentName = $"{group.Key.Nom} {group.Key.Prenom}",
                    ClassName = group.Key.NomClasse,
                    AbsenceCount = group.Count()
                })
                .OrderByDescending(vm => vm.AbsenceCount)
                .ToList();

            ViewBag.Classes = await _context.Classes
                .Select(c => new SelectListItem { Value = c.CodeClasse.ToString(), Text = c.NomClasse })
                .ToListAsync();

            ViewBag.Students = await _context.Etudiants
                .Select(e => new SelectListItem { Value = e.CodeEtudiant.ToString(), Text = $"{e.Nom} {e.Prenom}" })
                .ToListAsync();

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SelectedClassId = classeId;
            ViewBag.SelectedStudentId = studentId;

            return View(viewModel);
        }


        public async Task<IActionResult> AbsenceDetails(int studentId)
        {
         

            var absences = await _context.LignesFicheAbsence
                .Include(lfa => lfa.FicheAbsence)
                    .ThenInclude(fa => fa.Matiere)
                .Include(lfa => lfa.FicheAbsence)
                    .ThenInclude(fa => fa.Enseignant)
                .Include(lfa => lfa.FicheAbsence)
                    .ThenInclude(fa => fa.FichesAbsenceSeances)
                        .ThenInclude(fas => fas.Seance)
                .Where(lfa => lfa.CodeEtudiant == studentId)
                .OrderByDescending(lfa => lfa.FicheAbsence.DateJour)
                .ToListAsync();

            var student = await _context.Etudiants
                .FirstOrDefaultAsync(e => e.CodeEtudiant == studentId);

            ViewBag.StudentName = student != null
                ? $"{student.Nom} {student.Prenom}"
                : "Unknown Student";

            var viewModel = absences.Select(a => new AbsenceDetailsViewModel
            {
                Date = a.FicheAbsence.DateJour,
                SeanceName = a.FicheAbsence.FichesAbsenceSeances
                    .FirstOrDefault()?.Seance?.NomSeance ?? "N/A",
                TeacherName = $"{a.FicheAbsence.Enseignant.Nom} {a.FicheAbsence.Enseignant.Prenom}",
                SubjectName = a.FicheAbsence.Matiere.NomMatiere
            }).ToList();

            return View(viewModel);
        }


        public async Task<IActionResult> SubjectAbsenceDetails(int? studentId, int? matiereId)
        {
            if (!studentId.HasValue || !matiereId.HasValue)
            {
                await PopulateDropdowns();
                return View(new SubjectAbsenceDetailsViewModel
                {
                    AbsenceDetails = new List<AbsenceDetailsViewModel>(),
                    TotalAbsences = 0
                });
            }

            var absences = await _context.LignesFicheAbsence
                .Include(lfa => lfa.FicheAbsence)
                    .ThenInclude(fa => fa.Matiere)
                .Include(lfa => lfa.FicheAbsence)
                    .ThenInclude(fa => fa.Enseignant)
                .Include(lfa => lfa.FicheAbsence)
                    .ThenInclude(fa => fa.FichesAbsenceSeances)
                        .ThenInclude(fas => fas.Seance)
                .Where(lfa => lfa.CodeEtudiant == studentId &&
                             lfa.FicheAbsence.CodeMatiere == matiereId)
                .OrderByDescending(lfa => lfa.FicheAbsence.DateJour)
                .ToListAsync();

            var student = await _context.Etudiants
                .FirstOrDefaultAsync(e => e.CodeEtudiant == studentId);

            var matiere = await _context.Matieres
                .FirstOrDefaultAsync(m => m.CodeMatiere == matiereId);

            var viewModel = new SubjectAbsenceDetailsViewModel
            {
                StudentName = student != null ? $"{student.Nom} {student.Prenom}" : "Étudiant inconnu",
                SubjectName = matiere?.NomMatiere ?? "matiere inconnu",
                TotalAbsences = absences.Count,
                AbsenceDetails = absences.Select(a => new AbsenceDetailsViewModel
                {
                    Date = a.FicheAbsence.DateJour,
                    SeanceName = a.FicheAbsence.FichesAbsenceSeances
                        .FirstOrDefault()?.Seance?.NomSeance ?? "N/A",
                    TeacherName = $"{a.FicheAbsence.Enseignant.Nom} {a.FicheAbsence.Enseignant.Prenom}",
                    SubjectName = a.FicheAbsence.Matiere.NomMatiere
                }).ToList()
            };

            await PopulateDropdowns();
            ViewBag.SelectedStudentId = studentId;
            ViewBag.SelectedMatiereId = matiereId;

            return View(viewModel);
        }

        private async Task PopulateDropdowns()
        {
            ViewBag.Students = await _context.Etudiants
                .Select(e => new SelectListItem
                {
                    Value = e.CodeEtudiant.ToString(),
                    Text = $"{e.Nom} {e.Prenom}"
                })
                .ToListAsync();

            ViewBag.Matieres = await _context.Matieres
                .Select(m => new SelectListItem
                {
                    Value = m.CodeMatiere.ToString(),
                    Text = m.NomMatiere
                })
                .ToListAsync();
        }
        [HttpGet]
        public async Task<IActionResult> DailyAbsenceReport(DateTime? date)
        {
            var adminType = HttpContext.Session.GetString("UserType");
            if (adminType != "Admin")
            {
                return Unauthorized();
            }

            var selectedDate = date ?? DateTime.Today;

            var absences = await _context.LignesFicheAbsence
                .Include(lfa => lfa.FicheAbsence)
                    .ThenInclude(fa => fa.Matiere)
                .Include(lfa => lfa.FicheAbsence)
                    .ThenInclude(fa => fa.Enseignant)
                .Include(lfa => lfa.FicheAbsence)
                    .ThenInclude(fa => fa.FichesAbsenceSeances)
                        .ThenInclude(fas => fas.Seance)
                .Include(lfa => lfa.Etudiant)
                    .ThenInclude(e => e.Classe)
                .Where(lfa => lfa.FicheAbsence.DateJour.Date == selectedDate.Date)
                .OrderBy(lfa => lfa.Etudiant.Classe.NomClasse)
                .ThenBy(lfa => lfa.Etudiant.Nom)
                .ToListAsync();

            var viewModel = new DailyAbsenceReportViewModel
            {
                Date = selectedDate,
                AbsenceDetails = absences.Select(a => new DailyAbsenceDetail
                {
                    StudentName = $"{a.Etudiant.Nom} {a.Etudiant.Prenom}",
                    ClassName = a.Etudiant.Classe.NomClasse,
                    SubjectName = a.FicheAbsence.Matiere.NomMatiere,
                    SeanceName = a.FicheAbsence.FichesAbsenceSeances
                        .FirstOrDefault()?.Seance?.NomSeance ?? "N/A",
                    TeacherName = $"{a.FicheAbsence.Enseignant.Nom} {a.FicheAbsence.Enseignant.Prenom}"
                }).ToList()
            };

            var startOfWeek = selectedDate.AddDays(-(int)selectedDate.DayOfWeek);
            ViewBag.WeekDates = Enumerable.Range(0, 7)
                .Select(d => startOfWeek.AddDays(d))
                .ToList();

            return View(viewModel);
        }
    }

}




using Absence.Data;
using Absence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Absence.Cosntroller
{
    public class SubjectsController : Controller
    {
        private readonly AbsenceDbContext _context;

        public SubjectsController(AbsenceDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var subjects = await _context.Matieres.ToListAsync();
            return View(subjects);
        }

        public IActionResult Create()
        {
            var subject = new T_Matiere();
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomMatiere,NbrHeureCoursParSemaine,NbrHeureTDParSemaine,NbrHeureTPParSemaine")] T_Matiere subject)
        {
            if (ModelState.IsValid)
            {
                _context.Matieres.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }




        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Matieres.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodeMatiere,NomMatiere,NbrHeureCoursParSemaine,NbrHeureTDParSemaine,NbrHeureTPParSemaine")] T_Matiere subject)
        {
            if (id != subject.CodeMatiere) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSubject = await _context.Matieres.FindAsync(id);
                    if (existingSubject == null) return NotFound();

                    existingSubject.NomMatiere = subject.NomMatiere;
                    existingSubject.NbrHeureCoursParSemaine = subject.NbrHeureCoursParSemaine;
                    existingSubject.NbrHeureTDParSemaine = subject.NbrHeureTDParSemaine;
                    existingSubject.NbrHeureTPParSemaine = subject.NbrHeureTPParSemaine;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Matieres.Any(e => e.CodeMatiere == id))
                        return NotFound();
                    throw;
                }
            }
            return View(subject);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Matieres
                .FirstOrDefaultAsync(m => m.CodeMatiere == id);

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Matieres.FindAsync(id);
            if (subject != null)
            {
                try
                {
                    _context.Matieres.Remove(subject);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Impossible de supprimer. " + ex.Message);
                    return View(subject);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Matieres
                .FirstOrDefaultAsync(m => m.CodeMatiere == id);

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        private bool SubjectExists(int id)
        {
            return _context.Matieres.Any(e => e.CodeMatiere == id);
        }
    }
}
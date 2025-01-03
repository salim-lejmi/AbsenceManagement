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
        public async Task<IActionResult> Create(T_Enseignant teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Enseignants.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, T_Enseignant teacher)
        {
            if (id != teacher.CodeEnseignant) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Enseignants.Any(e => e.CodeEnseignant == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

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
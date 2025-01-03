using Absence.Data;
using Absence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var students = await _context.Etudiants.Include(s => s.Classe).ToListAsync();
            return View(students);
        }

        public IActionResult Create()
        {
            ViewData["Classes"] = _context.Classes.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(T_Etudiant student)
        {
            if (ModelState.IsValid)
            {
                _context.Etudiants.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Classes"] = _context.Classes.ToList();
            return View(student);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Etudiants.FindAsync(id);
            if (student == null) return NotFound();

            ViewData["Classes"] = _context.Classes.ToList();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, T_Etudiant student)
        {
            if (id != student.CodeEtudiant) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Etudiants.Any(e => e.CodeEtudiant == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Classes"] = _context.Classes.ToList();
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
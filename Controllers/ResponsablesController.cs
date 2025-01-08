using Absence.Data;
using Absence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Absence.Controllers
{
    public class ResponsablesController : Controller
    {
        private readonly AbsenceDbContext _context;

        public ResponsablesController(AbsenceDbContext context)
        {
            _context = context;
        }

        // GET: Responsables
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View(await _context.Responsables.ToListAsync());
        }

        // GET: Responsables/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Prenom,Mail,Password")] T_Responsable responsable)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                // Check if email already exists
                if (await _context.Responsables.AnyAsync(r => r.Mail == responsable.Mail))
                {
                    ModelState.AddModelError("Mail", "This email is already in use.");
                    return View(responsable);
                }

                _context.Add(responsable);
                await _context.SaveChangesAsync();

                // Create user account for the responsable
                var user = new T_User
                {
                    Email = responsable.Mail,
                    Password = responsable.Password,
                    UserType = "Responsable"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(responsable);
        }

        // GET: Responsables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var responsable = await _context.Responsables.FindAsync(id);
            if (responsable == null)
            {
                return NotFound();
            }
            return View(responsable);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodeResponsable,Nom,Prenom,Mail,Password")] T_Responsable responsable)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id != responsable.CodeResponsable)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if email exists for other responsables
                    var existingResponsable = await _context.Responsables
                        .FirstOrDefaultAsync(r => r.Mail == responsable.Mail && r.CodeResponsable != id);
                    if (existingResponsable != null)
                    {
                        ModelState.AddModelError("Mail", "This email is already in use.");
                        return View(responsable);
                    }

                    _context.Update(responsable);
                    await _context.SaveChangesAsync();

                    // Update user account
                    var user = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email == responsable.Mail);
                    if (user != null)
                    {
                        user.Password = responsable.Password;
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResponsableExists(responsable.CodeResponsable))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(responsable);
        }

        // GET: Responsables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var responsable = await _context.Responsables
                .FirstOrDefaultAsync(m => m.CodeResponsable == id);
            if (responsable == null)
            {
                return NotFound();
            }

            return View(responsable);
        }

        // POST: Responsables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var responsable = await _context.Responsables.FindAsync(id);
            if (responsable != null)
            {
                // Delete associated user account
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == responsable.Mail);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }

                _context.Responsables.Remove(responsable);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ResponsableExists(int id)
        {
            return _context.Responsables.Any(e => e.CodeResponsable == id);
        }
    }
}

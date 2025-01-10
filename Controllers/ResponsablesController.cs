﻿using Absence.Data;
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

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View(await _context.Responsables.ToListAsync());
        }

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
                if (await _context.Responsables.AnyAsync(r => r.Mail == responsable.Mail))
                {
                    ModelState.AddModelError("Mail", "Cet email est déjà utilisé.");
                    return View(responsable);
                }

                _context.Add(responsable);
                await _context.SaveChangesAsync();

                var user = new T_User
                {
                    Email = responsable.Mail,
                    Password = responsable.Password,
                    UserType = "Responsable",
                    ResponsableId = responsable.CodeResponsable

                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(responsable);
        }

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
                    var existingResponsable = await _context.Responsables
                        .FirstOrDefaultAsync(r => r.Mail == responsable.Mail && r.CodeResponsable != id);
                    if (existingResponsable != null)
                    {
                        ModelState.AddModelError("Mail", "Cet email est déjà utilisé.");
                        return View(responsable);
                    }

                    _context.Update(responsable);
                    await _context.SaveChangesAsync();

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

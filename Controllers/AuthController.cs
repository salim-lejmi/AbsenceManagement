using Absence.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Absence.Controllers
{
    public class AuthController : Controller
    {
        private readonly AbsenceDbContext _context;

        public AuthController(AbsenceDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserType") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View();
            }

            // Store user info in session
            HttpContext.Session.SetString("UserType", user.UserType);
            HttpContext.Session.SetInt32("UserId", user.UserId);

            if (user.TeacherId.HasValue)
                HttpContext.Session.SetInt32("TeacherId", user.TeacherId.Value);
            if (user.StudentId.HasValue)
                HttpContext.Session.SetInt32("StudentId", user.StudentId.Value);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

    using Absence.Data;
    using Absence.Models;
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
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ViewData["ErrorMessage"] = "Veuillez saisir votre adresse e-mail et votre mot de passe.";
                    return View();
                }

                try
                {
                    var user = await _context.Users
                        .Include(u => u.Teacher)
                        .Include(u => u.Student)
                        .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

                    if (user == null)
                    {
                        ViewData["ErrorMessage"] = "Email ou mot de passe invalide.";
                        return View();
                    }

                    HttpContext.Session.SetString("UserType", user.UserType);
                    HttpContext.Session.SetInt32("UserId", user.UserId);

                    switch (user.UserType)
                    {
                        case "Admin":
                            HttpContext.Session.SetString("UserName", "Admin");
                            break;

                        case "Teacher":
                            if (user.Teacher != null)
                            {
                                HttpContext.Session.SetInt32("TeacherId", user.TeacherId.Value);
                                HttpContext.Session.SetString("UserName", $"{user.Teacher.Nom} {user.Teacher.Prenom}");
                            }
                            break;

                        case "Student":
                            if (user.Student != null)
                            {
                                HttpContext.Session.SetInt32("StudentId", user.StudentId.Value);
                                HttpContext.Session.SetString("UserName", $"{user.Student.Nom} {user.Student.Prenom}");
                            }
                            break;

                        case "Responsable":
                            var responsable = await _context.Responsables
                                .FirstOrDefaultAsync(r => r.Mail == user.Email);
                            if (responsable != null)
                            {
                                HttpContext.Session.SetString("UserName", $"{responsable.Nom} {responsable.Prenom}");
                            }
                            break;
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = "Une erreur s'est produite lors de la connexion. Veuillez réessayer.";
                    return View();
                }
            }

            public IActionResult Logout()
            {
                try
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login");
                }
                catch (Exception)
                {
                    return RedirectToAction("Login");
                }
            }

        
        }
    }
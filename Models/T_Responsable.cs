using System.ComponentModel.DataAnnotations;

namespace Absence.Models
{
    public class T_Responsable
    {
        [Key]
        public int CodeResponsable { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}

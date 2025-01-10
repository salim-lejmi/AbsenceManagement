using System.ComponentModel.DataAnnotations;

namespace Absence.Models
{
    public class T_User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserType { get; set; } 
        public int? TeacherId { get; set; } 
        public int? StudentId { get; set; } 

        public virtual T_Enseignant Teacher { get; set; }
        public virtual T_Etudiant Student { get; set; }
    }
}


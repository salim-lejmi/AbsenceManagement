using System.ComponentModel.DataAnnotations;

namespace Absence.Models
{
    public class T_Responsable
    {
        [Key]
        public int CodeResponsable { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prenom est obligatoire")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Adresse e-mail non valide")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        public string Password { get; set; }
    }
}

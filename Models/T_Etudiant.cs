using Absence.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Absence.Models {

    public class T_Etudiant
    {
        public T_Etudiant()
        {
            LignesFicheAbsence = new HashSet<T_LigneFicheAbsence>();
        }

        [Key]
        public int CodeEtudiant { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Birth Date is required")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }

        [Required(ErrorMessage = "Class is required")]
        [Display(Name = "Class")]
        public int CodeClasse { get; set; }

        [Required(ErrorMessage = "Registration Number is required")]
        [Display(Name = "Registration Number")]
        public string NumInscription { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Adresse { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Mail { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Phone")]
        public string Tel { get; set; }

        public virtual T_Classe Classe { get; set; }
        public virtual ICollection<T_LigneFicheAbsence> LignesFicheAbsence { get; set; }
    }
}
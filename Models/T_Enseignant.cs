using Absence.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Absence.Models
{

    public class T_Enseignant
    {
        public T_Enseignant()
        {
            FichesAbsence = new HashSet<T_FicheAbsence>();
        }

        [Key]
        public int CodeEnseignant { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Recruitment Date is required")]
        [Display(Name = "Recruitment Date")]
        [DataType(DataType.Date)]
        public DateTime DateRecrutement { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Adresse { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Mail { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Phone")]
        public string Tel { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int CodeDepartement { get; set; }

        [Required(ErrorMessage = "Grade is required")]
        [Display(Name = "Grade")]
        public int CodeGrade { get; set; }

        public virtual T_Departement Departement { get; set; }
        public virtual T_Grade Grade { get; set; }
        public virtual ICollection<T_FicheAbsence> FichesAbsence { get; set; }
    }

}
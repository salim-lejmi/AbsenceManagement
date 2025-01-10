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

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prenom est obligatoire")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "La date de recrutement est obligatoire")]
        [Display(Name = "Recruitment Date")]
        [DataType(DataType.Date)]
        public DateTime DateRecrutement { get; set; }

        [Required(ErrorMessage = "L'adresse est obligatoire")]
        public string Adresse { get; set; }

        [EmailAddress(ErrorMessage = "Adresse e-mail non valide")]
        public string Mail { get; set; }

        [Phone(ErrorMessage = "Numéro de téléphone invalide")]
        [Display(Name = "Phone")]
        public string Tel { get; set; }

        [Required(ErrorMessage = "Le département est obligatoire")]
        [Display(Name = "Department")]
        public int CodeDepartement { get; set; }

        [Required(ErrorMessage = "Grade est obligatoire")]
        [Display(Name = "Grade")]
        public int CodeGrade { get; set; }
        [Required(ErrorMessage = "Le mot de passe est requis")]
        public string Password { get; set; }

        public virtual T_Departement Departement { get; set; }
        public virtual T_Grade Grade { get; set; }
        public virtual ICollection<T_FicheAbsence> FichesAbsence { get; set; }
    }

}
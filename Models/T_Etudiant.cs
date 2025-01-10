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

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prenom est obligatoire")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "La date de recrutement est obligatoire")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }

        [Required(ErrorMessage = "Classe est obligatoire")]
        [Display(Name = "Class")]
        public int CodeClasse { get; set; }

        [Required(ErrorMessage = "Le numéro d'enregistrement est requis")]
        [Display(Name = "Registration Number")]
        public string NumInscription { get; set; }

        [Required(ErrorMessage = "Addresse est obligatoire")]
        public string Adresse { get; set; }

        [EmailAddress(ErrorMessage = "Adresse e-mail non valide")]
        public string Mail { get; set; }

        [Phone(ErrorMessage = "Numéro de téléphone invalide")]
        [Display(Name = "Phone")]
        public string Tel { get; set; }

        public virtual T_Classe Classe { get; set; }
        public virtual ICollection<T_LigneFicheAbsence> LignesFicheAbsence { get; set; }
    }
}
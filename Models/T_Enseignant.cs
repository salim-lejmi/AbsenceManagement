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
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateRecrutement { get; set; }
        public string Adresse { get; set; }
        public string Mail { get; set; }
        public string Tel { get; set; }
        [ForeignKey("Departement")]
        public int CodeDepartement { get; set; }
        public virtual T_Departement Departement { get; set; }
        [ForeignKey("Grade")]
        public int CodeGrade { get; set; }
        public virtual T_Grade Grade { get; set; }
        public virtual ICollection<T_FicheAbsence> FichesAbsence { get; set; }
    }
}
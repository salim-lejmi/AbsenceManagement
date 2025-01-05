using Absence.Models;
using System.ComponentModel.DataAnnotations;
namespace Absence.Models
{

    public class T_Matiere
    {
        public T_Matiere()
        {
            FichesAbsence = new HashSet<T_FicheAbsence>();
        }

        [Key]
        public int CodeMatiere { get; set; }
        [Required(ErrorMessage = "Subject name is required.")]
        public string NomMatiere { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Course hours must be greater than 0.")]
        public int NbrHeureCoursParSemaine { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "TD hours cannot be negative.")]
        public int NbrHeureTDParSemaine { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "TP hours cannot be negative.")]
        public int NbrHeureTPParSemaine { get; set; }
        public virtual ICollection<T_FicheAbsence> FichesAbsence { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Absence.Models
{
    public class T_Matiere
    {
        [Key]
        public int CodeMatiere { get; set; } 
        public string NomMatiere { get; set; }
        public int NbrHeureCoursParSemaine { get; set; }
        public int NbrHeureTDParSemaine { get; set; }
        public int NbrHeureTPParSemaine { get; set; }
        public virtual ICollection<T_FicheAbsence> FichesAbsence { get; set; }
    }
}

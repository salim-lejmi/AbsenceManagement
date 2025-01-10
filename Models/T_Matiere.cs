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
        [Required(ErrorMessage = "Le nom du matier est obligatoire.")]
        public string NomMatiere { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Les heures de cours doivent être supérieures à 0.")]
        public int NbrHeureCoursParSemaine { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Les heures TD ne peuvent pas être négatives.")]
        public int NbrHeureTDParSemaine { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Les heures TP ne peuvent pas être négatives.")]
        public int NbrHeureTPParSemaine { get; set; }
        public virtual ICollection<T_FicheAbsence> FichesAbsence { get; set; }
    }
}
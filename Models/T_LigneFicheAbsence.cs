using System.ComponentModel.DataAnnotations.Schema;

namespace Absence.Models
{
    public class T_LigneFicheAbsence
    {
        [ForeignKey("FicheAbsence")]
        public int CodeFicheAbsence { get; set; }  
        public virtual T_FicheAbsence FicheAbsence { get; set; }

        [ForeignKey("Etudiant")]
        public int CodeEtudiant { get; set; }  
        public virtual T_Etudiant Etudiant { get; set; }
    }

}

using System.ComponentModel.DataAnnotations.Schema;

namespace Absence.Models
{
    public class T_FicheAbsenceSeance
    {
        public int CodeFicheAbsenceSeance { get; set; }  

        [ForeignKey("FicheAbsence")]
        public int CodeFicheAbsence { get; set; }  
        public virtual T_FicheAbsence FicheAbsence { get; set; }

        [ForeignKey("Seance")]
        public int CodeSeance { get; set; }  
        public virtual T_Seance Seance { get; set; }
    }


}

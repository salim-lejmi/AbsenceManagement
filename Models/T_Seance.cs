using System.ComponentModel.DataAnnotations;

namespace Absence.Models
{
    public class T_Seance
    {
        [Key]
        public int CodeSeance { get; set; }  
        public string NomSeance { get; set; }
        public DateTime HeureDebut { get; set; }
        public DateTime HeureFin { get; set; }
        public virtual ICollection<T_FicheAbsenceSeance> FichesAbsenceSeances { get; set; }
    }
}

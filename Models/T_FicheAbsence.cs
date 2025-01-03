using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Absence.Models
{
    public class T_FicheAbsence
    {
        [Key]
        public int CodeFicheAbsence { get; set; }  
        public DateTime DateJour { get; set; }

        [ForeignKey("Matiere")]
        public int CodeMatiere { get; set; }  
        public virtual T_Matiere Matiere { get; set; }

        [ForeignKey("Enseignant")]
        public int CodeEnseignant { get; set; }  
        public virtual T_Enseignant Enseignant { get; set; }

        [ForeignKey("Classe")]
        public int CodeClasse { get; set; }    
        public virtual T_Classe Classe { get; set; }

        public virtual ICollection<T_FicheAbsenceSeance> FichesAbsenceSeances { get; set; }
        public virtual ICollection<T_LigneFicheAbsence> LignesFicheAbsence { get; set; }
    }

}

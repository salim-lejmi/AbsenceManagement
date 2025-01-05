using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Absence.Models
{
    public class T_Classe
    {
        [Key]
        public int CodeClasse { get; set; } 
        public string NomClasse { get; set; }

        [ForeignKey("Groupe")]
        public int CodeGroupe { get; set; } 
        public virtual T_Groupe Groupe { get; set; }

        [ForeignKey("Departement")]
        public int CodeDepartement { get; set; } 
        public virtual T_Departement Departement { get; set; }

        public virtual ICollection<T_Etudiant> Etudiants { get; set; }
        public virtual ICollection<T_FicheAbsence> FichesAbsence { get; set; }
    }

}

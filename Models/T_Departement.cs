using System.ComponentModel.DataAnnotations;

namespace Absence.Models
{
    public class T_Departement
    {
        [Key]
        public int CodeDepartement { get; set; } 
        public string NomDepartement { get; set; }
        public virtual ICollection<T_Classe> Classes { get; set; }
        public virtual ICollection<T_Enseignant> Enseignants { get; set; }
    }
}

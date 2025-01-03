using System.ComponentModel.DataAnnotations;

namespace Absence.Models
{
    public class T_Grade
    {
        [Key]
        public int CodeGrade { get; set; }  
        public string NomGrade { get; set; }
        public virtual ICollection<T_Enseignant> Enseignants { get; set; }
    }

}

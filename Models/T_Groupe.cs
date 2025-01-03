using System.ComponentModel.DataAnnotations;

namespace Absence.Models
{
    public class T_Groupe
    {
        [Key]
        public int CodeGroupe { get; set; }  
        public string NomGroupe { get; set; }
        public virtual ICollection<T_Classe> Classes { get; set; }
    }
}

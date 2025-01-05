using Absence.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Absence.Models { 

public class T_Etudiant
{
    public T_Etudiant()
    {
        LignesFicheAbsence = new HashSet<T_LigneFicheAbsence>();
    }

    [Key]
    public int CodeEtudiant { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public DateTime DateNaissance { get; set; }
    [ForeignKey("Classe")]
    public int CodeClasse { get; set; }
    public virtual T_Classe Classe { get; set; }
    public string NumInscription { get; set; }
    public string Adresse { get; set; }
    public string Mail { get; set; }
    public string Tel { get; set; }
    public virtual ICollection<T_LigneFicheAbsence> LignesFicheAbsence { get; set; }
}
}
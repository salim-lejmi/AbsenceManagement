using Absence.Models;

namespace Absence.ViewModels
{
    public class MarkAbsenceViewModel
    {
        public MarkAbsenceViewModel()
        {
            Classes = new List<T_Classe>();
            Matieres = new List<T_Matiere>();
            Absences = new List<StudentAbsence>();
            Date = DateTime.Now;
        }

        public List<T_Classe> Classes { get; set; }
        public List<T_Matiere> Matieres { get; set; }
        public List<StudentAbsence> Absences { get; set; }
        public DateTime Date { get; set; }
        public int SelectedClassId { get; set; }
        public int SelectedSubjectId { get; set; }
    }

    public class StudentAbsence
    {
        public int StudentId { get; set; }
        public bool IsAbsent { get; set; }
    }
}
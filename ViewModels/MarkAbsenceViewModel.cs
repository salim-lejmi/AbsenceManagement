using Absence.Models;

namespace Absence.ViewModels
{
    public class MarkAbsenceViewModel
    {
        public List<T_Classe> Classes { get; set; }
        public List<T_Seance> Seances { get; set; }
        public List<StudentAbsence> Absences { get; set; }
    }

    public class StudentAbsence
    {
        public int StudentId { get; set; }
        public int FicheAbsenceId { get; set; }
        public bool IsAbsent { get; set; }
    }
}
namespace Absence.ViewModels
{
    public class SubjectAbsenceDetailsViewModel
    {
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public int TotalAbsences { get; set; }
        public List<AbsenceDetailsViewModel> AbsenceDetails { get; set; }

    }
}

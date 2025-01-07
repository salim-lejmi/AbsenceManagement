namespace Absence.ViewModels
{
    public class StudentAbsenceReportViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }  // Added this
        public int AbsenceCount { get; set; }

    }
}
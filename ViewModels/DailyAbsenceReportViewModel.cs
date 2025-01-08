using Absence.Models;
namespace Absence.ViewModels

{
    public class DailyAbsenceReportViewModel
    {
        public DateTime Date { get; set; }
        public List<DailyAbsenceDetail> AbsenceDetails { get; set; } = new List<DailyAbsenceDetail>();
    }

    public class DailyAbsenceDetail
    {
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string SeanceName { get; set; }
        public string TeacherName { get; set; }
    }
}

﻿using Absence.Models;

namespace Absence.ViewModels
{
    public class MarkAbsenceViewModel
    {
        public MarkAbsenceViewModel()
        {
            Classes = new List<T_Classe>();
            Matieres = new List<T_Matiere>();
            Seances = new List<T_Seance>();  // Add this
            Absences = new List<StudentAbsence>();
            Date = DateTime.Now;
        }

        public List<T_Classe> Classes { get; set; }
        public List<T_Matiere> Matieres { get; set; }
        public List<T_Seance> Seances { get; set; }  // Add this
        public List<StudentAbsence> Absences { get; set; }
        public DateTime Date { get; set; }
        public int SelectedClassId { get; set; }
        public int SelectedSubjectId { get; set; }
        public int SelectedSeanceId { get; set; }  // Add this
    }

    public class StudentAbsence
    {
        public int StudentId { get; set; }
        public bool IsAbsent { get; set; }
    }
}
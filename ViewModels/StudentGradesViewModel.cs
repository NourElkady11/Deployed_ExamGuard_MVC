using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class StudentGradesViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public List<CourseGradesViewModel> Courses { get; set; } = new List<CourseGradesViewModel>();
        public int TotalCompletedExams { get; set; }
        public double? AverageGrade { get; set; }
    }

    public class CourseGradesViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public List<ExamGradeViewModel> Exams { get; set; } = new List<ExamGradeViewModel>();
    }

    public class ExamGradeViewModel
    {
        public int ExamId { get; set; }
        public string ExamSubject { get; set; }
        public string ExamCode { get; set; }
        public DateTime ExamDate { get; set; }
        public int Grade { get; set; }
        public string Status { get; set; }
        public string StatusClass => Status == "Completed" ? "bg-success" : "bg-warning";
    }
}

using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Models
{
    public class Exam
    {
        public int Id { get; set; }

        public string? Subject { get; set; }

        public string? Code { get; set; }

        public TimeOnly Duration { get; set;}

        public DateOnly Date { get; set; }

        public ICollection<Question>? Questions { get; set; } = new List<Question>();

        public Course? Course { get; set; }

        public int? CourseId { get; set; }

        public int? TotalGrade { get; set; }

        public ICollection<StudentExam>? studentExams { get; set; } = new List<StudentExam>();

        public ICollection<StudentAnswer>? StudentAnswers { get; set; } = new List<StudentAnswer>();

        public ICollection<CheatingReport>? CheatingReports { get; set; } = new List<CheatingReport>();

    }
}

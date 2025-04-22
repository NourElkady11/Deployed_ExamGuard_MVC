using Data.Models;
using DataAccess_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ExamReportsViewModel
    {
        public Exam Exam { get; set; }
        public List<StudentReportSummary> ReportSummary { get; set; }
        public IEnumerable<CheatingReport> DetailedReports { get; set; }
    }

    public class StudentReportSummary
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public List<DetectionCount> DetectionCounts { get; set; }
        public int TotalDetections { get; set; }
    }

    public class DetectionCount
    {
        public string DetectionType { get; set; }
        public int Count { get; set; }
    }
}

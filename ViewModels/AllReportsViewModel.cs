using DataAccess_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AllReportsViewModel
    {
        public List<ExamCheatingReportSummary> ExamReportSummaries { get; set; }
        public int TotalReports { get; set; }
        public int TotalExamsWithReports { get; set; }
    }

    public class ExamCheatingReportSummary
    {
        public Exam Exam { get; set; }
        public int ReportCount { get; set; }
        public int StudentsWithReports { get; set; }
        public List<DetectionCount> TopDetectionTypes { get; set; }
    }
}

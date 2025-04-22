using Data.Models;
using DataAccess_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class StudentExamReportViewModel
    {
        public Exam Exam { get; set; }
        public Student Student { get; set; }
        public IEnumerable<CheatingReport> Reports { get; set; }
    }
}

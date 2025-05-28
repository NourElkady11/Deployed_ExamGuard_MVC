using Data.Models;
using DataAccess_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation_Layer.ViewModels
{
    public class ExamReviewViewModel
    {
        public Exam Exam { get; set; }
        public StudentExam StudentExam { get; set; }
        public Dictionary<int?, int?> StudentAnswers { get; set; }

    }
}

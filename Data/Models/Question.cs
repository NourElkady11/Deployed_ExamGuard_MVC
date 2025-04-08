using DataAccess_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string? QuestionText { get; set; }
        public string? Answer { get; set; }

        public Exam? Exam { get; set; }
        public int? ExamId { get; set; }

        public ICollection<Choice> Choices { get; set; } = new List<Choice>();

        public ICollection<StudentAnswer>? StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}

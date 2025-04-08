using DataAccess_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class StudentAnswer
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public int ChoiceId { get; set; }

        public Student? Student { get; set; }
        public Exam? Exam { get; set; }
        public Question? Question { get; set; }
        public Choice? Choice { get; set; }
    }
}

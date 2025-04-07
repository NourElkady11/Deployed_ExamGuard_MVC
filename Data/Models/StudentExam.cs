using DataAccess_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class StudentExam
    {
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public int? Grade { get; set; }
        public string? Status { get; set; }= "Not-Completed";

        public Student? student { get; set; }
        public Exam? exam { get; set; }

    }
}

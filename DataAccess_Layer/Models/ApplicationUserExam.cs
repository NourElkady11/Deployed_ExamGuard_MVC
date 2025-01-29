using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Models
{
    public class ApplicationUserExam
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }


        public Exam Exam { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int UserID { get; set; }
        public int ExamId { get; set; }
    }
}

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

        public string Subject { get; set; }

        public string Code { get; set; }

        public TimeOnly Duration { get; set;}

        public DateOnly Date { get; set; }

  /*      public ICollection<string> Questions { get; set; }= new List<string>();

        public ICollection<string> Choices { get; set; }= new List<string>();*/

        public Course? Course { get; set; }

        public int? CourseId { get; set; }

    }
}

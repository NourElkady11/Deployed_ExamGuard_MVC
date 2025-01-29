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
        public string Name { get; set; }
        public string Type { get; set; }
        public TimeOnly Duration { get; set;}
        public DateOnly Date { get; set; }
    }
}

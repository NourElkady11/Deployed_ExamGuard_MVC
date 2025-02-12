using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
 
        public string Email { get; set; }

        public string phone { get; set; }

        public string? address { get; set; }

        public bool IsActive { get; set; }

        public string? ImageName { get; set; }

        public ICollection<CourseStudent>? CourseStudents { get; set; } = new List<CourseStudent>();

    }
}

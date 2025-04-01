using DataAccess_Layer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Range(0, 500, ErrorMessage = "you are out of the range")]
        public string? Code { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Display(Name = "Created at")]
        public DateTime DateTime { get; set; }= DateTime.Now;

        public ICollection<CourseStudent>? CourseStudents { get; set; } = new List<CourseStudent>();

        public ICollection<Exam>? Exams { get; set; }= new List<Exam>();

        public int? SuperVisorId { get; set; }
        public SuperVisor? superVisor { get; set; }
       
    }
}

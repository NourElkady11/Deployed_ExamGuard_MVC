using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace Presentation_Layer.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string? Code { get; set; }

        [Display(Name = "Created at")]
        public DateTime DateTime { get; set; } = DateTime.Now;

        public int? SuperVisorId { get; set; }

        public List<SelectListItem> Supervisors { get; set; } = new List<SelectListItem>();
    }
}

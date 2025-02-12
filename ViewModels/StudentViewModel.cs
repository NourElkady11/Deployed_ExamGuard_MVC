using Data.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Presentation_Layer.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
 
        [Phone]
        public string phone { get; set; }

        public string? address { get; set; }

        public bool IsActive { get; set; }

        public IFormFile? Image { get; set; }

        public string? ImageName { get; set; }

        public ICollection<CourseStudent>? CourseStudents { get; set; } = new List<CourseStudent>();


    }
}

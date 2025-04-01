using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels
{
    public class ExamViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Code { get; set; }
        public string ? Status { get; set; }
        [Required(ErrorMessage = "Duration is required")]
        [Display(Name = "Duration (minutes)")]
        public int DurationMinutes { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public int? SupervisorId { get; set; }
        public int TotalGrade { get; set; }
        [Required(ErrorMessage = "Course is required")]
        public int CourseId { get; set; }

        public string? CourseName { get; set; }

        public List<QuestionViewModel>? Questions { get; set; } = new List<QuestionViewModel>();
        public SelectList? CourseSelectList { get; set; }
        public List<SelectListItem> Supervisors { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Courses { get; set; } = new List<SelectListItem>();
    }
}

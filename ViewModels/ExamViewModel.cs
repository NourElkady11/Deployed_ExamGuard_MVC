using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation_Layer.ViewModels
{
    public class ExamViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public TimeOnly Duration { get; set; }
        public DateOnly Date { get; set; }
        public int? CourseId { get; set; }
        public int? SupervisorId { get; set; }
        public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
        public List<SelectListItem> Supervisors { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Courses { get; set; } = new List<SelectListItem>();
    }
}

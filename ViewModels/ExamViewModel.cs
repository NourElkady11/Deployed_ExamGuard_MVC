namespace Presentation_Layer.ViewModels
{
    public class ExamViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Code { get; set; }
        public int Duration { get; set; }
        public DateTime StartTime { get; set; }
        public string Status { get; set; }

        public ICollection<string> Questions { get; set; } = new List<string>();

        public ICollection<string> Choices { get; set; } = new List<string>();
    }
}

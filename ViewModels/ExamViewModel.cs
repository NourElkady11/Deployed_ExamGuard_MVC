namespace Presentation_Layer.ViewModels
{
    public class ExamViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public int Duration { get; set; }
        public DateTime StartTime { get; set; }
        public string Status { get; set; }
    }
}

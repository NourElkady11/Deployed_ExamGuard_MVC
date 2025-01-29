using AutoMapper;
using DataAccess_Layer.Repositories;
using Services.Abstraction;
using Presentation_Layer.ViewModels;

namespace Services
{
    public class ExamService(IUnitOfWork unitOfWork,IMapper mapper):IExamService
    {

        public async Task<IEnumerable<ExamViewModel>> GetExams()
        {
            var Exams = new List<ExamViewModel>()
            {
               new ExamViewModel
               {
                   Id = 1,
                   Title = "Mathematics Exam",
                   Subject = "Mathematics",
                   Duration = 60,
                   StartTime = DateTime.Now.AddHours(1),
                   Status = "upcoming"
               },
               new ExamViewModel
               {
                   Id = 2,
                   Title = "Physics Exam",
                   Subject = "Physics",
                   Duration = 90,
                   StartTime = DateTime.Now.AddHours(2),
                   Status = "completed"
               }

            };

            return Exams;
        }

    }
}

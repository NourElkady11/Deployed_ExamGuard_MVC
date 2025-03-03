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
                   Subject = "Data-Structure",
                   Duration = TimeOnly.MaxValue,
                   Date = DateOnly.MaxValue,
                   Status = "Pending"

               },
               new ExamViewModel
               {
                   Id = 2,
                   Subject = "Physics",
                  Duration = TimeOnly.MaxValue,
                   Date = DateOnly.MaxValue,
                   Status = "completed"
               }

            };

            return Exams;
        }

    }
}

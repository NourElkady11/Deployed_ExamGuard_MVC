using AutoMapper;
using DataAccess_Layer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels;
using Services.Abstraction;

namespace Presentation_Layer.Controllers
{
    [Authorize(Roles = "SuperVisor")]
    public class SuperVisorController(HttpClient _httpClient, IUnitOfWork unitOfWork, IMapper mapper) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllCourses(string userEmail)
        {
            
            var supervisor=await unitOfWork.SuperVisorRepository.GetSuperVisorWithEmail(userEmail);
            if (supervisor == null) return NotFound();
            
            var CoursesOfSuperVisor=await unitOfWork.CoursesRepo.GetSuperVisorCourses(supervisor.Id);

         
            return View(CoursesOfSuperVisor);
        }

        public async Task<IActionResult> GetCourseExams(int courseid)
        {
            var exams = await unitOfWork.ExamRepository.GetCourseExamsAsync(courseid);
            ViewBag.mycourseId = courseid;
            return View(exams);
        }

        public async Task<IActionResult> ShowExam(int id)
        {
            var exam = await unitOfWork.ExamRepository.GetExamWithQuestionsAndChoicesAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            else
            {
                var viewModel = new ExamViewModel
                {
                    Id = exam.Id,
                    Subject = exam.Subject,
                    Code = exam.Code,
                    DurationMinutes = exam.Duration.Hour * 60 + exam.Duration.Minute,
                    Date = exam.Date.ToDateTime(TimeOnly.MinValue),
                    CourseId = exam.CourseId ?? 0,
                    CourseName = exam.Course?.Name,
                    TotalGrade = exam.TotalGrade ?? 0,
                    Questions = new List<QuestionViewModel>()
                };
                foreach (var question in exam.Questions)
                {
                    var questionVM = new QuestionViewModel
                    {
                        Id = question.Id,
                        QuestionText = question.QuestionText,
                        Choices = new List<ChoiceViewModel>(),
                        CorrectChoiceIndex = 0
                    };

                    int choiceIndex = 0;
                    foreach (var choice in question.Choices)
                    {
                        questionVM.Choices.Add(new ChoiceViewModel
                        {
                            Id = choice.Id,
                            ChoiceText = choice.ChoiceText
                        });

                        // Set the correct choice index
                        if (choice.ChoiceText == question.Answer)
                        {
                            questionVM.CorrectChoiceIndex = choiceIndex;
                        }
                        choiceIndex++;
                    }

                    viewModel.Questions.Add(questionVM);
                }
                return View(viewModel);

            }

        }



    }
}

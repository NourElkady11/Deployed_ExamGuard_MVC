using AutoMapper;
using DataAccess_Layer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation_Layer.ViewModels;
using Services.Abstraction;
using ViewModels;

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
            ViewBag.UserEmail = userEmail;

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
        public async Task<IActionResult> GetAllReports()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email);
            var supervisor = await unitOfWork.SuperVisorRepository.GetSuperVisorWithEmail(email.Value);
            var courses = await unitOfWork.CoursesRepo.GetSuperVisorCourses(supervisor.Id);

            // Create lists to collect all data across all courses and exams
            List<ExamCheatingReportSummary> allExamReportSummaries = new List<ExamCheatingReportSummary>();
            int totalReportsCount = 0;

            foreach (var course in courses)
            {
                var courseExams = await unitOfWork.ExamRepository.GetCourseExamsAsync(course.Id);

                foreach (var exam in course.Exams)
                {
                    var allReports = await unitOfWork.CheatingReportRepository.GetReportsByExamIdAsync(exam.Id);
                    totalReportsCount += allReports.Count();

                    if (allReports.Any())
                    {
                        // Group all reports by exam and then by student
                        var examReportSummary = new ExamCheatingReportSummary
                        {
                            Exam = courseExams.FirstOrDefault(e => e.Id == exam.Id),
                            ReportCount = allReports.Count(),
                            StudentsWithReports = allReports.GroupBy(r => r.StudentId).Count(),
                            TopDetectionTypes = allReports.GroupBy(r => r.DetectionType)
                                .OrderByDescending(dg => dg.Count())
                                .Take(3)
                                .Select(dg => new DetectionCount
                                {
                                    DetectionType = dg.Key,
                                    Count = dg.Count()
                                })
                                .ToList()
                        };

                        allExamReportSummaries.Add(examReportSummary);
                    }
                }
            }

            // Now create the viewModel outside the loops with all the collected data
            var viewModel = new AllReportsViewModel
            {
                ExamReportSummaries = allExamReportSummaries.OrderByDescending(s => s.ReportCount).ToList(),
                TotalReports = totalReportsCount,
                TotalExamsWithReports = allExamReportSummaries.Count
            };

            if (viewModel.TotalReports == 0)
            {
                return View(new AllReportsViewModel());
            }

            return View(viewModel);
        }


        // Add these methods to SuperVisorController

        public async Task<IActionResult> ViewExamReports(int examId)
        {
            var exam = await unitOfWork.ExamRepository.GetAsync(examId);
            if (exam == null)
            {
                return NotFound();
            }
            var reports = await unitOfWork.CheatingReportRepository.GetReportsByExamIdAsync(examId);

            // Group reports by student and detection type for summary
            var reportSummary = reports
                .GroupBy(r => new { r.StudentId, r.StudentName, r.StudentEmail })
                .Select(g => new StudentReportSummary
                {
                    StudentId = g.Key.StudentId??0,
                    StudentName = g.Key.StudentName,
                    StudentEmail = g.Key.StudentEmail,
                    DetectionCounts = g.GroupBy(r => r.DetectionType)
                        .Select(dg => new DetectionCount
                        {
                            DetectionType = dg.Key,
                            Count = dg.Count()
                        }).ToList(),
                    TotalDetections = g.Count()
                })
                .OrderByDescending(s => s.TotalDetections)
                .ToList();

            var viewModel = new ExamReportsViewModel
            {
                Exam = exam,
                ReportSummary = reportSummary,
                DetailedReports = reports
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ViewStudentExamReport(int examId, int studentId)
        {
            var exam = await unitOfWork.ExamRepository.GetAsync(examId);
            var student = await unitOfWork.StudentsRepo.GetAsync(studentId);

            if (exam == null || student == null)
            {
                return NotFound();
            }

            var reports = await unitOfWork.CheatingReportRepository.GetReportsByExamAndStudentAsync(examId, studentId);

            var viewModel = new StudentExamReportViewModel
            {
                Exam = exam,
                Student = student,
                Reports = reports
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ViewReportImage(string imagePath)
        {
            try
            {
                // Fetch the image from Python API
                var response = await _httpClient.GetAsync($"http://localhost:5000/get_snapshot/{imagePath}");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var imageData = JsonConvert.DeserializeObject<dynamic>(responseBody);

                    ViewBag.ImageData = imageData.image.ToString();
                    return View();
                }
                else
                {
                    return StatusCode(500, "Failed to fetch image.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }



    }
}

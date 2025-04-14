using AutoMapper;
using Data.Models;
using DataAccess_Layer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation_Layer.ViewModels;
using Services;
using Services.Abstraction;
using System.Net.Http;

namespace Presentation_Layer.Controllers
{
    public class StudentController(HttpClient _httpClient,IUnitOfWork unitOfWork,IMapper mapper) : Controller
    {
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            var courses = await unitOfWork.CoursesRepo.GetCourseWithExamssAsync();
            return View(courses);
        }

        public async Task<IActionResult> GetCourseExams(int courseid)
        {
            var exams = await unitOfWork.ExamRepository.GetCourseExamsAsync(courseid);
            var email=User.FindFirst(System.Security.Claims.ClaimTypes.Email);
            var student = await unitOfWork.StudentsRepo.GetStudentWithEmail(email.Value);
            var userId = student.Id;
         /*   var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);*/

            var studentExams = await unitOfWork.StudentExamRepository.GetStudentExamsAsync(userId);
            ViewBag.StudentExams = studentExams.ToDictionary(se => se.ExamId, se => se);
            ViewBag.MyCourseId = courseid;

            return View(exams);
        }


        public async Task<IActionResult> GoToExam(int examId)
        {
            var exam = await unitOfWork.ExamRepository.GetExamWithQuestionsAndChoicesAsync(examId);
            ViewData["ExamId"] = examId;
            return View(exam); 
        }


        public async Task<IActionResult> ReviewExam(int examId)
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email);
            var student = await unitOfWork.StudentsRepo.GetStudentWithEmail(email.Value);
            var userId = student.Id;
         /*   var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);*/
            var exam = await unitOfWork.ExamRepository.GetExamWithQuestionsAndChoicesAsync(examId);
            var studentExam = await unitOfWork.StudentExamRepository.GetAsync(userId, examId);

            if (exam == null || studentExam == null || studentExam.Status != "Completed")
            {
                return NotFound();
            }

            // Load student answers
            var studentAnswers = await unitOfWork.StudentAnswerRepository.GetStudentExamAnswersAsync(userId, examId);

            var reviewViewModel = new ExamReviewViewModel
            {
                Exam = exam,
                StudentExam = studentExam,
                StudentAnswers = studentAnswers.ToDictionary(sa => sa.QuestionId, sa => sa.ChoiceId)
            };

            return View(reviewViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> GetDetectionResults(int examId)
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:5000/start_exam");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var detections = JsonConvert.DeserializeObject<dynamic>(responseBody);

                    // Extract labels from the current frame's detections
                    var labels = new List<string>();
                    foreach (var detection in detections.detections)
                    {
                        var label = detection.labels[0].ToString();
                        labels.Add(label);
                    }

                    return Json(new { detections = labels });
                }
                else
                {
                    return StatusCode(500, "Failed to fetch detection results.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitExam(int examId, [FromBody] Dictionary<string, string> examData)
        {
            try
            {
                if (examData == null)
                {
                    return BadRequest("No exam data provided");
                }

                await StopExam(examId);

                // Get the current user ID
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email);
                var student = await unitOfWork.StudentsRepo.GetStudentWithEmail(email.Value);
                var userId = student.Id;
                /* var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);*/

                // Get the exam with questions and choices
                var exam = await unitOfWork.ExamRepository.GetExamWithQuestionsAndChoicesAsync(examId);
                if (exam == null)
                    return StatusCode(500, "Exam not found");

                // Calculate grade
                int correctAnswers = 0;
                int totalQuestions = exam.Questions.Count;


/*                await unitOfWork.StudentAnswerRepository.DeleteStudentExamAnswersAsync(userId, examId);
*/

                // Process each question and save the student's answers
                foreach (var question in exam.Questions)
                {
                    string questionKey = $"q{question.Id}";
                    if (examData.ContainsKey(questionKey))
                    {
                        int choiceId = int.Parse(examData[questionKey]);
                        var selectedChoice = question.Choices.FirstOrDefault(c => c.Id == choiceId);

                        // Save the student's answer
                        var studentAnswer = new StudentAnswer
                        {
                            StudentId = userId,
                            ExamId = examId,
                            QuestionId = question.Id,
                            ChoiceId = choiceId
                        };

                        await unitOfWork.StudentAnswerRepository.CreateAsync(studentAnswer);

                        if (selectedChoice != null && selectedChoice.ChoiceText == question.Answer)
                        {
                            correctAnswers++;
                        }
                    }
                }

                // Calculate percentage grade
                int grade = (totalQuestions > 0) ? (correctAnswers * 100) / totalQuestions : 0;

                // Create or update StudentExam record
                var studentExam = await unitOfWork.StudentExamRepository.GetAsync(userId, examId);
                if (studentExam == null)
                {
                    studentExam = new StudentExam
                    {
                        StudentId = userId,
                        ExamId = examId,
                        Grade = grade,
                        Status = "Completed"
                    };
                    await unitOfWork.StudentExamRepository.CreateAsync(studentExam);
                }
                else
                {
                    studentExam.Grade = grade;
                    studentExam.Status = "Completed";
                    unitOfWork.StudentExamRepository.Update(studentExam);
                }

                await unitOfWork.SaveChangesAsync();

                return Json(new { success = true, message = "Exam submitted successfully", grade = grade });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error submitting exam: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> StopExam(int examId)
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:5000/stop_exam");
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return StatusCode(500, "Failed to stop exam monitoring.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }

           
        }

        public async Task<IActionResult> ExamComplete()
        {
            return View();
        }

   /*     [HttpPost]
        public async Task<IActionResult> ReceiveDetection([FromBody] dynamic detection)
        {
            try
            {
                // Extract label from the detection object and show alert
                var label = detection.label[0].ToString(); // Extract the label
                Console.WriteLine($"Cheating Detected: {label}");  // Log the detection
                TempData["detections"] = label;

                // You can return the detection label or other data
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Error processing detection");
            }
        }*/


        public async Task<IActionResult> GetAllGrades()
        {
            return View();

        }


        public async Task<IActionResult> Profile()
        {
            return View();
        }

        public async Task<IActionResult> EditProfile()
        {
            return View();
        }

    }
}
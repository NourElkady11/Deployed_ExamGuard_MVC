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
using ViewModels;

namespace Presentation_Layer.Controllers
{
    public class StudentController(HttpClient _httpClient, IUnitOfWork unitOfWork, IMapper mapper) : Controller
    {

        private static Dictionary<int, int> behavioralViolations = new Dictionary<int, int>();
        private const int MAX_BEHAVIORAL_VIOLATIONS = 10;
        private static Dictionary<int, Dictionary<string, DateTime>> lastDetectionTimes = new Dictionary<int, Dictionary<string, DateTime>>();
        private const int DETECTION_COOLDOWN_SECONDS = 3;

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            var courses = await unitOfWork.CoursesRepo.GetCourseWithExamssAsync();
            return View(courses);
        }

        public async Task<IActionResult> GetCourseExams(int courseid)
        {
            var exams = await unitOfWork.ExamRepository.GetCourseExamsAsync(courseid);
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email);
            var student = await unitOfWork.StudentsRepo.GetStudentWithEmail(email.Value);
            var userId = student.Id;
            /*   var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);*/

            var studentExams = await unitOfWork.StudentExamRepository.GetStudentExamsAsync(userId);
            ViewBag.StudentExams = studentExams.ToDictionary(se => se.ExamId, se => se);
            ViewBag.MyCourseId = courseid;

            return View(exams);
        }

        public async Task<IActionResult> GetAllGrades()
        {
            // Get current student information
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email);
            var student = await unitOfWork.StudentsRepo.GetStudentWithEmail(email.Value);

            if (student == null)
            {
                return NotFound("Student not found.");
            }


            var studentExams = await unitOfWork.StudentExamRepository.GetStudentExamsAsync(student.Id);


            var studentCourses = await unitOfWork.CoursesRepo.GetCourseWithExamssAsync();


            var viewModel = new StudentGradesViewModel
            {
                StudentId = student.Id,
                StudentName = student.Name,
                StudentEmail = student.Email,
                Courses = new List<CourseGradesViewModel>(),
                TotalCompletedExams = studentExams.Count(se => se.Status == "Completed"),
                AverageGrade = studentExams.Any(se => se.Status == "Completed")
                    ? studentExams.Where(se => se.Status == "Completed").Average(se => se.Grade)
                    : 0
            };

            // Group exams by course
            foreach (var course in studentCourses)
            {
                if (course == null) continue;

                // Get all exams for this course
                var courseExams = await unitOfWork.ExamRepository.GetCourseExamsAsync(course.Id);

                var courseViewModel = new CourseGradesViewModel
                {
                    CourseId = course.Id,
                    CourseName = course.Name,
                    CourseCode = course.Code,
                    Exams = new List<ExamGradeViewModel>()
                };
                // Add exam details
                foreach (var exam in courseExams)
                {
                    var studentExam = studentExams.FirstOrDefault(se => se.ExamId == exam.Id);

                    var examViewModel = new ExamGradeViewModel
                    {
                        ExamId = exam.Id,
                        ExamSubject = exam.Subject,
                        ExamCode = exam.Code,
                        ExamDate = exam.Date.ToDateTime(TimeOnly.MinValue),
                        Grade = studentExam?.Grade ?? 0,
                        Status = studentExam?.Status ?? "Not Taken"
                    };

                    courseViewModel.Exams.Add(examViewModel);
                }

                // Only add courses that have exams
                if (courseViewModel.Exams.Any())
                {
                    viewModel.Courses.Add(courseViewModel);
                }
            }

            return View(viewModel);
        }


        public async Task<IActionResult> GoToExam(int examId)
        {
            var exam = await unitOfWork.ExamRepository.GetExamWithQuestionsAndChoicesAsync(examId);
            ViewData["ExamId"] = examId;
            return View(exam);
        }

        [HttpPost]
        public async Task<IActionResult> RecordTabSwitch([FromBody] TabSwitchData tabSwitchData)
        {
            try
            {
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email);
                var studentName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                var student = await unitOfWork.StudentsRepo.GetStudentWithEmail(email.Value);
                var studentId = student.Id;

                var report = new CheatingReport
                {
                    StudentId = studentId,
                    ExamId = tabSwitchData.ExamId,
                    StudentName = studentName,
                    StudentEmail = email.Value,
                    DetectionTime = DateTime.Now,
                    DetectionType = "Tab Switching",
                    TabSwitchCount = tabSwitchData.ViolationCount,
                    ImagePath = null // No image for tab switching
                };

                await unitOfWork.CheatingReportRepository.CreateAsync(report);
                await unitOfWork.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error recording tab switch: {ex.Message}");
            }
        }

        public class TabSwitchData
        {
            public int ExamId { get; set; }
            public int ViolationCount { get; set; }
        }

        public async Task<IActionResult> ReviewExam(int examId)
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email);
            var student = await unitOfWork.StudentsRepo.GetStudentWithEmail(email.Value);
            var userId = student.Id;
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
                    var detectionData = JsonConvert.DeserializeObject<dynamic>(responseBody);

                    // Extract detections
                    var detections = new List<dynamic>();
                    if (detectionData.detections != null && detectionData.detections.Count > 0)
                    {
                        // Get student information
                        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email);
                        var studentName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                        var student = await unitOfWork.StudentsRepo.GetStudentWithEmail(email.Value);
                        var studentId = student.Id;

                        // Initialize detection tracking for this student if not exists
                        if (!lastDetectionTimes.ContainsKey(studentId))
                        {
                            lastDetectionTimes[studentId] = new Dictionary<string, DateTime>();
                        }

                        foreach (var detection in detectionData.detections)
                        {
                            var label = detection.label.ToString();
                            var timestamp = DateTime.Parse(detection.timestamp.ToString());
                            var snapshotFile = detection.snapshot.ToString();

                            // Check if enough time has passed since the last detection of this type
                            bool shouldRecord = true;
                            if (lastDetectionTimes[studentId].ContainsKey(label))
                            {
                                var timeDifference = timestamp - lastDetectionTimes[studentId][label];
                                if (timeDifference.TotalSeconds < DETECTION_COOLDOWN_SECONDS)
                                {
                                    shouldRecord = false;
                                    Console.WriteLine($"Skipping duplicate detection: {label} - Time difference: {timeDifference.TotalSeconds} seconds");
                                }
                            }

                            if (shouldRecord)
                            {
                                // Update the last detection time for this type
                                lastDetectionTimes[studentId][label] = timestamp;

                                // Create a new cheating report
                                var report = new CheatingReport
                                {
                                    StudentId = studentId,
                                    ExamId = examId,
                                    StudentName = studentName,
                                    StudentEmail = email.Value,
                                    DetectionTime = timestamp,
                                    DetectionType = label,
                                    ImagePath = snapshotFile
                                };

                                await unitOfWork.CheatingReportRepository.CreateAsync(report);
                                detections.Add(new
                                {
                                    label = label,
                                    isBehavioral = IsBehavioralDetection(label),
                                    imagePath = snapshotFile
                                });
                            }
                        }

                        if (detections.Any()) // Only save if there are actual detections to save
                        {
                            await unitOfWork.SaveChangesAsync();
                        }
                    }

                    return Json(new { detections = detections });
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


        private bool IsBehavioralDetection(string detectionType)
        {
            var behavioralTypes = new[] {
                "Student Absence",
                "Looking Away",
                "Posture Change",
                "Head Movement",
                "Multiple Persons"
            };
            return behavioralTypes.Contains(detectionType);
        }

        [HttpPost]
        public async Task<IActionResult> RecordFocusLoss([FromBody] FocusLossData focusData)
        {
            try
            {
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email);
                var studentName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                var student = await unitOfWork.StudentsRepo.GetStudentWithEmail(email.Value);
                var studentId = student.Id;

                var report = new CheatingReport
                {
                    StudentId = studentId,
                    ExamId = focusData.ExamId,
                    StudentName = studentName,
                    StudentEmail = email.Value,
                    DetectionTime = DateTime.Now,
                    DetectionType = "Focus Loss",
                    ImagePath = focusData.ImagePath
                };

                await unitOfWork.CheatingReportRepository.CreateAsync(report);
                await unitOfWork.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error recording focus loss: {ex.Message}");
            }
        }

        public class FocusLossData
        {
            public int ExamId { get; set; }
            public string ImagePath { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> RecordBehavioralViolation([FromBody] BehavioralViolationData violationData)
        {
            try
            {
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email);
                var studentName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                var student = await unitOfWork.StudentsRepo.GetStudentWithEmail(email.Value);
                var studentId = student.Id;

                // Initialize detection tracking for this student if not exists
                if (!lastDetectionTimes.ContainsKey(studentId))
                {
                    lastDetectionTimes[studentId] = new Dictionary<string, DateTime>();
                }

                // Check for duplicate detection
                var currentTime = DateTime.Now;
                bool shouldRecord = true;

                if (lastDetectionTimes[studentId].ContainsKey(violationData.DetectionType))
                {
                    var timeDifference = currentTime - lastDetectionTimes[studentId][violationData.DetectionType];
                    if (timeDifference.TotalSeconds < DETECTION_COOLDOWN_SECONDS)
                    {
                        shouldRecord = false;
                    }
                }

                if (!shouldRecord)
                {
                    return Json(new { success = false, message = "Duplicate detection ignored" });
                }

                // Update the last detection time
                lastDetectionTimes[studentId][violationData.DetectionType] = currentTime;

                // Track violations per student
                if (!behavioralViolations.ContainsKey(studentId))
                {
                    behavioralViolations[studentId] = 0;
                }
                behavioralViolations[studentId]++;

                var report = new CheatingReport
                {
                    StudentId = studentId,
                    ExamId = violationData.ExamId,
                    StudentName = studentName,
                    StudentEmail = email.Value,
                    DetectionTime = currentTime,
                    DetectionType = violationData.DetectionType,
                    ImagePath = violationData.ImagePath
                };

                await unitOfWork.CheatingReportRepository.CreateAsync(report);
                await unitOfWork.SaveChangesAsync();

                // Check if max violations reached
                bool shouldSubmit = behavioralViolations[studentId] >= MAX_BEHAVIORAL_VIOLATIONS;

                return Json(new
                {
                    success = true,
                    violationCount = behavioralViolations[studentId],
                    shouldSubmit = shouldSubmit
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error recording behavioral violation: {ex.Message}");
            }
            
        }


        public class BehavioralViolationData
        {
            public int ExamId { get; set; }
            public string DetectionType { get; set; }
            public string ImagePath { get; set; }
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
                var exam = await unitOfWork.ExamRepository.GetExamWithQuestionsAndChoicesAsync(examId);
                if (exam == null)
                    return StatusCode(500, "Exam not found");
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
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email);
                var student = await unitOfWork.StudentsRepo.GetStudentWithEmail(email.Value);

                // Clear detection tracking for this student
                if (lastDetectionTimes.ContainsKey(student.Id))
                {
                    lastDetectionTimes[student.Id].Clear();
                }

                if (behavioralViolations.ContainsKey(student.Id))
                {
                    behavioralViolations.Remove(student.Id);
                }

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
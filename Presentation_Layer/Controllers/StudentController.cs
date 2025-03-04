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
    public class StudentController(IExamService examService, IServiceManger serviceManger, HttpClient _httpClient,IUnitOfWork unitOfWork,IMapper mapper) : Controller
    {
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            var courses = await unitOfWork.CoursesRepo.GetCourseWithSuperVisorssAsync();
            return View(courses);
           

        }

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


        [HttpPost]
        public async Task<IActionResult> StartExam(int examId)
        {
            try
            {
                // Redirect to GoToExam action with the examId
                return RedirectToAction("GoToExam", new { examId = examId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        public async Task<IActionResult> GoToExam(int examId)
        {
            // Render the GoToExam page immediately
            ViewData["ExamId"] = examId;
            return View(examId); // Pass the examId to the view
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
                // Stop the exam monitoring first
                await StopExam(examId);

                // Save the exam answers
              /*  await examService.SubmitExam(examId, examData);*/

                return Json(new { success = true, message = "Exam submitted successfully" });
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

        [HttpPost]
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
        }
    }
}
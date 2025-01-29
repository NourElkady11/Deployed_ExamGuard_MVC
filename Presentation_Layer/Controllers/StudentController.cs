using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Abstraction;

namespace Presentation_Layer.Controllers
{
    public class StudentController(IExamService examService,IServiceManger serviceManger) : Controller
    {
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            var exams = await serviceManger.examService.GetExams();
            return View(exams);
           
        }

        public async Task<IActionResult> GetAllGrades()
        {
            return View();
        }



        [HttpPost]
        public IActionResult StartExam(int examId)
        {
         
            try
            {
                // Updated paths
                var pythonExePath = @"C:\Users\Actel\anaconda3\envs\ElkadyEnv\python.exe";
                var pythonFilePath = @"C:\Users\Actel\Downloads\YOLOv10\webcam-interface.py";

                // Ensure paths are valid
                if (!System.IO.File.Exists(pythonExePath))
                {
                    return StatusCode(500, $"Python executable not found at {pythonExePath}");
                }
                if (!System.IO.File.Exists(pythonFilePath))
                {
                    return StatusCode(500, $"Python script not found at {pythonFilePath}");
                }

                // Process configuration
                var processStartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = pythonExePath,
                    Arguments = $"\"{pythonFilePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = @"C:\Users\Actel\Downloads\YOLOv10"
                };

                // Start process
                using (var process = System.Diagnostics.Process.Start(processStartInfo))
                {
                    process.WaitForExit();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (process.ExitCode != 0)
                    {
                        Console.WriteLine($"Error: {error}");
                        return StatusCode(500, $"Python script error: {error}");
                    }

                    return Ok($"Exam {examId} started successfully. Output: {output}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Exception occurred: {ex.Message}");
            }
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using DataAccess_Layer.Models;
using DataAccess_Layer.Repositories;
using DataAccess_Layer.Data;

namespace Presentation_Layer.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ShowWelcome = HttpContext.Session.GetString("ShowWelcome");
            ViewBag.WelcomeId = HttpContext.Session.GetString("WelcomeId");
            HttpContext.Session.Remove("ShowWelcome");

            // ✅ Load user image if logged in but image not in session
       /*     if (User.Identity.IsAuthenticated && string.IsNullOrEmpty(HttpContext.Session.GetString("UserImage")))
            {
                await LoadUserImage();
            }*/

            return View();
        }

        private async Task LoadUserImage()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Check if user is a student
                    var student = await _unitOfWork.StudentsRepo.GetStudentWithEmail(user.Email);
                    if (student?.ImageName != null)
                    {
                        HttpContext.Session.SetString("UserImage", student.ImageName);
                        return;
                    }

                    // Check if user is a supervisor
                    var supervisor = await _unitOfWork.SuperVisorRepository.GetSuperVisorWithEmail(user.Email);
                    if (supervisor?.ImageName != null)
                    {
                        HttpContext.Session.SetString("UserImage", supervisor.ImageName);
                        return;
                    }

                    // Check if user is admin
                    if (user.Email == "nourel2ady11@gmail.com")
                    {
                        HttpContext.Session.SetString("UserImage", "ADMIN.jpg");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user image");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Welcome()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
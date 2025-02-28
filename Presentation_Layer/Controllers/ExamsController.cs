using DataAccess_Layer.Data;
using DataAccess_Layer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation_Layer.ViewModels;
using Services.Abstraction;

namespace Presentation_Layer.Controllers
{
    [Authorize(Roles = "Admin,SuperVisor")]
    public class ExamsController(UserManager<ApplicationUser> userManager, IServiceManger serviceManger,IUnitOfWork unitOfWork) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExamViewModel exam)
        {
            return View();
        }
        /// <summary>
        /// ///////////////
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ExamViewModel exam)
        {
            return View();
        }

        /// <summary>
     
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> Delete()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ExamViewModel exam)
        {
            return View();
        }







    }
}

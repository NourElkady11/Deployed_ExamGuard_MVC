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
    [Authorize(Roles = "SuperVisor")]
    public class ExamsController(UserManager<ApplicationUser> userManager, IServiceManger serviceManger,IUnitOfWork unitOfWork) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }


    }
}

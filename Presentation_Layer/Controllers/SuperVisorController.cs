using AutoMapper;
using DataAccess_Layer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;

namespace Presentation_Layer.Controllers
{
    [Authorize(Roles = "SuperVisor")]
    public class SuperVisorController(IExamService examService, IServiceManger serviceManger, HttpClient _httpClient, IUnitOfWork unitOfWork, IMapper mapper) : Controller
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


    }
}

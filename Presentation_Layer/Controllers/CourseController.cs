using DataAccess_Layer.Data;
using DataAccess_Layer.Repositories;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation_Layer.ViewModels;
using Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Presentation_Layer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public CourseController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _unitOfWork.CoursesRepo.GetCourseWithExamssAsync();
            return View(courses);
        }

        public async Task<IActionResult> GetCourseExams(int courseid)
        {
            var exams = await _unitOfWork.ExamRepository.GetCourseExamsAsync(courseid);
            ViewBag.mycourseId = courseid;
            return View(exams);
        }

     

            public async Task<IActionResult> Create()
            {
            var viewModel = new CourseViewModel
            {
                Supervisors = await GetSupervisorsAsSelectListItems()
            };
                 return View(viewModel);
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
         
                var course = new Course
                {
                    Name = courseViewModel.Name,
                    Code = courseViewModel.Code,
                    DateTime = DateTime.Now,
                    SuperVisorId = courseViewModel.SuperVisorId
                };

            
                await _unitOfWork.CoursesRepo.CreateAsync(course);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

          
            courseViewModel.Supervisors = await GetSupervisorsAsSelectListItems();
            return View(courseViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var course = await _unitOfWork.CoursesRepo.GetAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = new CourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Code = course.Code,
                DateTime = course.DateTime,
                SuperVisorId = course.SuperVisorId,
                Supervisors = await GetSupervisorsAsSelectListItems()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseViewModel courseViewModel)
        {
            if (id != courseViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var course = await _unitOfWork.CoursesRepo.GetAsync(id);
                if (course == null)
                {
                    return NotFound();
                }

            
                course.Name = courseViewModel.Name;
                course.Code = courseViewModel.Code;
                course.SuperVisorId = courseViewModel.SuperVisorId;

                _unitOfWork.CoursesRepo.Update(course);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }


            courseViewModel.Supervisors = await GetSupervisorsAsSelectListItems();
            return View(courseViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var course = await _unitOfWork.CoursesRepo.GetCourseWithExamAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _unitOfWork.CoursesRepo.GetCourseWithExamAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            if (course.Exams != null && course.Exams.Any())
            {
                TempData["ErrorMessage"] = "Cannot delete course that has exams assigned to it. Delete the exams first.";
                return RedirectToAction(nameof(Index));
            }

            _unitOfWork.CoursesRepo.Delete(course);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private async Task<List<SelectListItem>> GetSupervisorsAsSelectListItems()
        {
            var supervisors = await _unitOfWork.SuperVisorRepository.GetAllAsync();


            var supervisorUsers = await _userManager.GetUsersInRoleAsync("Supervisor");
            var supervisorEmails = supervisorUsers.Select(u => u.Email).ToList();

            return supervisors
                .Where(s => supervisorEmails.Contains(s.Email))
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.FirstName} {s.LastName} ({s.Email})"
                })
                .ToList();
        }

 
    }
}
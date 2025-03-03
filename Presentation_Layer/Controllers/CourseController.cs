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
        private readonly IServiceManger _serviceManger;
        private readonly IUnitOfWork _unitOfWork;

        public CourseController(UserManager<ApplicationUser> userManager, IServiceManger serviceManger, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _serviceManger = serviceManger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _unitOfWork.CoursesRepo.GetCourseWithSuperVisorssAsync();
            return View(courses);
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
                // Create new course entity
                var course = new Course
                {
                    Name = courseViewModel.Name,
                    Code = courseViewModel.Code,
                    DateTime = DateTime.Now,
                    SuperVisorId = courseViewModel.SuperVisorId
                };

                // Add course to repository
                await _unitOfWork.CoursesRepo.CreateAsync(course);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If we get here, something failed - redisplay form
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

                // Update course properties
                course.Name = courseViewModel.Name;
                course.Code = courseViewModel.Code;
                course.SuperVisorId = courseViewModel.SuperVisorId;

                // Update in repository
                _unitOfWork.CoursesRepo.Update(course);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If we get here, something failed - redisplay form
            courseViewModel.Supervisors = await GetSupervisorsAsSelectListItems();
            return View(courseViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var course = await _unitOfWork.CoursesRepo.GetAsync(id);
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
            var course = await _unitOfWork.CoursesRepo.GetAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            // Before deleting the course, check if it has any exams
            var courseWithExams = await _unitOfWork.CoursesRepo.GetCourseWithExamsAsync(id);
            if (courseWithExams.Exams != null && courseWithExams.Exams.Any())
            {
                // Redirect back to course list with error message
                TempData["ErrorMessage"] = "Cannot delete course that has exams assigned to it. Delete the exams first.";
                return RedirectToAction(nameof(Index));
            }

            // Delete the course
            _unitOfWork.CoursesRepo.Delete(course);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Helper method to get supervisors as select list items
        private async Task<List<SelectListItem>> GetSupervisorsAsSelectListItems()
        {
            var supervisors = await _unitOfWork.SuperVisorRepository.GetAllAsync();

            // Get users with Supervisor role
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

        // Action to create exam for a specific course
        public async Task<IActionResult> CreateExam(int courseId)
        {
            var course = await _unitOfWork.CoursesRepo.GetAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            // Redirect to the exam creation form with the course ID pre-filled
            TempData["CourseId"] = courseId;
            TempData["CourseName"] = course.Name;
            return RedirectToAction("Create", "Exams");
        }
    }
}
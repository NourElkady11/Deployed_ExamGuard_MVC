using Data.Models;
using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using DataAccess_Layer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Presentation_Layer.ViewModels;
using Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation_Layer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExamsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
      /*  private readonly IServiceManger _serviceManger;*/
        private readonly IUnitOfWork _unitOfWork;

        public ExamsController(UserManager<ApplicationUser> userManager, /*IServiceManger serviceManger,*/ IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
        /*    _serviceManger = serviceManger;*/
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var exams = await _unitOfWork.ExamRepository.GetAllAsync();
            return View(exams);
        }

        public async Task<IActionResult> Create(int courseId)
        {
            var course = await _unitOfWork.CoursesRepo.GetAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = new ExamViewModel
            {
                CourseId = courseId,
                CourseName = course.Name,
                Date = DateTime.Now.Date,
                DurationMinutes = 60,
                Questions = new List<QuestionViewModel>
                {
                    new QuestionViewModel
                    {
                        Choices = new List<ChoiceViewModel>
                        {
                            new ChoiceViewModel { ChoiceText = "" },
                            new ChoiceViewModel { ChoiceText = "" },
                            new ChoiceViewModel { ChoiceText = "" },
                            new ChoiceViewModel { ChoiceText = "" }
                        }
                    }
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamViewModel examViewModel)
        {
            if (ModelState.IsValid)
            {
                var course = await _unitOfWork.CoursesRepo.GetAsync(examViewModel.CourseId);
                if (course == null)
                {
                    return NotFound();
                }

                // Create the Exam entity
                var exam = new Exam
                {
                    Subject = examViewModel.Subject,
                    Code = examViewModel.Code,
                    Duration = TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(examViewModel.DurationMinutes)),
                    Date = DateOnly.FromDateTime(examViewModel.Date),
                    CourseId = examViewModel.CourseId,
                    TotalGrade = examViewModel.Questions.Count // Each question is worth 1 point
                };

                // Create Questions and Choices
                foreach (var questionVM in examViewModel.Questions)
                {
                    var question = new Question
                    {
                        QuestionText = questionVM.QuestionText,
                        Answer = questionVM.Choices[questionVM.CorrectChoiceIndex].ChoiceText
                    };

                    foreach (var choiceVM in questionVM.Choices)
                    {
                        var choice = new Choice
                        {
                            ChoiceText = choiceVM.ChoiceText,
                            Question = question
                        };
                        question.Choices.Add(choice);
                    }

                    exam.Questions.Add(question);
                }

                await _unitOfWork.ExamRepository.CreateAsync(exam);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction("Index", "Course");
            }
            else
            {
               
            }

            // If we got to here, something failed, redisplay form
            var courseForRedisplay = await _unitOfWork.CoursesRepo.GetAsync(examViewModel.CourseId);
            examViewModel.CourseName = courseForRedisplay?.Name;

            return View(examViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var exam = await _unitOfWork.ExamRepository.GetExamWithQuestionsAndChoicesAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            var viewModel = new ExamViewModel
            {
                Id = exam.Id,
                Subject = exam.Subject,
                Code = exam.Code,
                DurationMinutes = exam.Duration.Hour * 60 + exam.Duration.Minute,
                Date = exam.Date.ToDateTime(TimeOnly.MinValue),
                CourseId = exam.CourseId ?? 0,
                CourseName = exam.Course?.Name,
                TotalGrade = exam.TotalGrade ?? 0,
                Questions = new List<QuestionViewModel>()
            };

            // Add questions to the view model
            foreach (var question in exam.Questions)
            {
                var questionVM = new QuestionViewModel
                {
                    Id = question.Id,
                    QuestionText = question.QuestionText,
                    Choices = new List<ChoiceViewModel>(),
                    CorrectChoiceIndex = 0
                };

                int choiceIndex = 0;
                foreach (var choice in question.Choices)
                {
                    questionVM.Choices.Add(new ChoiceViewModel
                    {
                        Id = choice.Id,
                        ChoiceText = choice.ChoiceText
                    });

                    // Set the correct choice index
                    if (choice.ChoiceText == question.Answer)
                    {
                        questionVM.CorrectChoiceIndex = choiceIndex;
                    }
                    choiceIndex++;
                }

                viewModel.Questions.Add(questionVM);
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExamViewModel examViewModel)
        {
            if (id != examViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var exam = await _unitOfWork.ExamRepository.GetExamWithQuestionsAndChoicesAsync(id);
                    if (exam == null)
                    {
                        return NotFound();
                    }

                    // Update exam properties
                    exam.Subject = examViewModel.Subject;
                    exam.Code = examViewModel.Code;
                    exam.Duration = TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(examViewModel.DurationMinutes));
                    exam.Date = DateOnly.FromDateTime(examViewModel.Date);
                    exam.TotalGrade = examViewModel.Questions.Count; // Update total grade based on question count

                  
                    foreach (var question in exam.Questions.ToList())
                    {
                        foreach (var choice in question.Choices.ToList())
                        {
                            question.Choices.Remove(choice);
                        }
                        exam.Questions.Remove(question);
                    }

                    foreach (var questionVM in examViewModel.Questions)
                    {
                        var question = new Question
                        {
                            QuestionText = questionVM.QuestionText,
                            Answer = questionVM.Choices[questionVM.CorrectChoiceIndex].ChoiceText,
                            ExamId = exam.Id
                        };

                        foreach (var choiceVM in questionVM.Choices)
                        {
                            var choice = new Choice
                            {
                                ChoiceText = choiceVM.ChoiceText,
                                Question = question
                            };
                            question.Choices.Add(choice);
                        }

                        exam.Questions.Add(question);
                    }

                    _unitOfWork.ExamRepository.Update(exam);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ExamExists(examViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Course");
            }

            // If we got here, something failed, redisplay form
            var course = await _unitOfWork.CoursesRepo.GetAsync(examViewModel.CourseId);
            examViewModel.CourseName = course?.Name;

            return View(examViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var exam = await _unitOfWork.ExamRepository.GetExamWithQuestionsAndChoicesAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _unitOfWork.ExamRepository.GetAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            _unitOfWork.ExamRepository.Delete(exam);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index", "Course");
        }

        private async Task<bool> ExamExists(int id)
        {
            var exam = await _unitOfWork.ExamRepository.GetAsync(id);
            return exam != null;
        }
    }
}
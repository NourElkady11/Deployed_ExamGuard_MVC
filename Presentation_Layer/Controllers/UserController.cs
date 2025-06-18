using AutoMapper;
using DataAccess_Layer.Repositories;
using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Presentation_Layer.Utilities;
using Presentation_Layer.ViewModels;
using DataAccess_Layer.Data;
using Data.Models;

namespace Presentation_Layer.Controllers
{
	[Authorize(Roles = "Admin")]	
	public class UserController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : Controller
	{
		public async Task<IActionResult> Index(string email)
		{
			
			if (string.IsNullOrWhiteSpace(email))
			{
				var user = await userManager.Users.Select(user => new UserViewModel
				{
					Id = user.Id,
					Email = user.Email,
					FirstName = user.Firstname,
					LastName = user.Lastname,
					Username = user.UserName,
					Roles = userManager.GetRolesAsync(user).GetAwaiter().GetResult()

				}).ToListAsync();

				return View(user);
			}
			else
			{
				var EmailTrimed=email.Trim();
				var user = await userManager.FindByEmailAsync(EmailTrimed);
				if (user is null)
				{
					ModelState.AddModelError(string.Empty, "User not found with the provided email address.");
					return View(Enumerable.Empty<UserViewModel>());
				}

				List<UserViewModel> userViewModels = new List<UserViewModel>();
				var model = new UserViewModel()
				{
					Id = user.Id,
					Email = user.Email,
					FirstName = user.Firstname,
					LastName = user.Lastname,
					Username = user.UserName,
					Roles = userManager.GetRolesAsync(user).GetAwaiter().GetResult()
				};
				userViewModels.Add(model);
				return View(userViewModels);

			}
		}
		[HttpPost]
		public async Task<IActionResult> Create(UserViewModel userViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(userViewModel);
			}
			else
			{
				try
				{
					var user = new ApplicationUser()
					{
						Firstname = userViewModel.FirstName,
						Lastname = userViewModel.LastName,
						UserName = userViewModel.Username,
						Email = userViewModel.Email,
					};
                    var Result = userManager.CreateAsync(user, userViewModel.Password).Result;
					var supervisor = new SuperVisor()
					{
						FirstName = userViewModel.FirstName,
						LastName = userViewModel.LastName,
						Username = userViewModel.Username,
						Email = userViewModel.Email,

					};
					if(userViewModel.Image is not null)
					{
						supervisor.ImageName = await DocumentSetting.uploadFile(userViewModel.Image, "Images");
                    }
				


					await unitOfWork.SuperVisorRepository.CreateAsync(supervisor);
					await unitOfWork.SaveChangesAsync();



                    return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}



			}
			return View(userViewModel);
		}



		public async Task<IActionResult> Details(string id) => await UserHandler(id, nameof(Details));

		public async Task<IActionResult> Edit(string id) => await UserHandler(id, nameof(Edit));

		public async Task<IActionResult> Delete(string id) => await UserHandler(id, nameof(Delete));

		public async Task<IActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Edit(string id, UserViewModel userViewModel)
		{
			if (id != userViewModel.Id)
			{
				return BadRequest();
			}
			else
			{
				if (!ModelState.IsValid)
				{
					return View(userViewModel);
				}
				else
				{
					try
					{
						var user = await userManager.FindByIdAsync(id);
						if (user is null)
						{
							return BadRequest();
						}
						else
						{
							user.Firstname = userViewModel.FirstName;
							user.Lastname = userViewModel.LastName;
							user.UserName = userViewModel.Username;
							user.Email = userViewModel.Email;
							await userManager.UpdateAsync(user);
							return RedirectToAction(nameof(Index));

						}
					}
					catch (Exception ex)
					{
						ModelState.AddModelError(string.Empty, ex.Message);
						throw new Exception(ex.ToString());
					}
				}
			}
			return View(userViewModel);

		}

		[HttpPost]
		public async Task<IActionResult> Delete(string id, UserViewModel userViewModel)
		{
			if (id != userViewModel.Id)
			{
				return BadRequest();
			}
			else
			{
				if (!ModelState.IsValid)
				{
					return View(userViewModel);
				}
				else
				{
					try
					{
						var user = await userManager.FindByIdAsync(id);
						if (user is null)
						{
							return NotFound();
						}
						else
						{
							await userManager.DeleteAsync(user);
							return RedirectToAction(nameof(Index));

						}
					}
					catch (Exception ex)
					{
						ModelState.AddModelError(string.Empty, ex.Message);


					}
				}
			}
			return View(userViewModel);



		}





		public async Task<IActionResult> UserHandler(string id, string viewname)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return BadRequest();
			}
			else
			{
				var user = await userManager.FindByIdAsync(id);
				if (user is null) { return NotFound(); };
				var model = new UserViewModel
				{
					Id = user.Id,
					FirstName = user?.Firstname ?? string.Empty,
					LastName = user?.Lastname ?? string.Empty,
					Username = user?.UserName ?? string.Empty,
					Email = user?.Email,
					Roles = userManager.GetRolesAsync(user).GetAwaiter().GetResult()

				};
				return View(viewname, model);

			}
		}



	}
}

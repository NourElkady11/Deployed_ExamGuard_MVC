using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation_Layer.ViewModels;
using System.Data;

namespace Presentation_Layer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var role = await roleManager.Roles.Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name

                }).ToListAsync();

                return View(role);
            }
            else
            {
                var role = await roleManager.FindByNameAsync(name);
                if (role is null)
                {
                    return View(Enumerable.Empty<RoleViewModel>);
                }

                var model = new RoleViewModel()
                {
                    Id = role.Id,
                    Name = role.Name,

                };

                return View(model);

            }
        }


        public async Task<IActionResult> Details(string id) => await UserHandler(id, nameof(Details));

        public async Task<IActionResult> Edit(string id) => await UserHandler(id, nameof(Edit));

        public async Task<IActionResult> Delete(string id) => await UserHandler(id, nameof(Delete));

        public async Task<IActionResult> Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            var role = new IdentityRole
            {
                Name = roleViewModel.Name,
            };
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(roleViewModel);

        }



        [HttpPost]
        public async Task<IActionResult> Edit(string id, RoleViewModel roleViewModel)
        {
            if (id != roleViewModel.Id)
            {
                return BadRequest();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(roleViewModel);
                }
                else
                {
                    try
                    {
                        var role = await roleManager.FindByIdAsync(id);
                        if (role is null)
                        {
                            return BadRequest();
                        }
                        else
                        {
                            role.Name = roleViewModel.Name;
                            await roleManager.UpdateAsync(role);
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
            return View(roleViewModel);



        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, RoleViewModel roleViewModel)
        {
            if (id != roleViewModel.Id)
            {
                return BadRequest();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(roleViewModel);
                }
                else
                {
                    try
                    {
                        var role = await roleManager.FindByIdAsync(id);
                        if (role is null)
                        {
                            return BadRequest();
                        }
                        else
                        {
                            role.Name = roleViewModel.Name;
                            await roleManager.DeleteAsync(role);
                            return RedirectToAction(nameof(Index));

                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);


                    }
                }
            }
            return View(roleViewModel);



        }


        public async Task<IActionResult> UserHandler(string id, string viewname)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            else
            {
                var role = await roleManager.FindByIdAsync(id);
                if (role is null) { return NotFound(); };
                var model = new RoleViewModel()
                {
                    Id = role.Id,
                    Name = role.Name,

                };
                return View(viewname, model);

            }
        }




        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null) { return NotFound(); }
            ViewBag.RoleId = roleId;
            var users = await userManager.Users.ToListAsync();
            var usersInRole = new List<UserInRoleViewModel>();

            foreach (var user in users)
            {
                var UserInRole = new UserInRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsInRnole = await userManager.IsInRoleAsync(user, role.Name)
                };
                usersInRole.Add(UserInRole);
            }


            return View(usersInRole);

        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UserInRoleViewModel> users)
        {

            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null) { return NotFound(); }

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await userManager.FindByIdAsync(user.UserId);
                    if (user.IsInRnole && !await userManager.IsInRoleAsync(appUser, role.Name))
                    {
                        await userManager.AddToRoleAsync(appUser, role.Name);
                    }
                    if (!user.IsInRnole && await userManager.IsInRoleAsync(appUser, role.Name))
                    {
                        await userManager.RemoveFromRoleAsync(appUser, role.Name);
                    }


                }
                return RedirectToAction("Index", "User");

               /* return RedirectToAction(nameof(Edit), new { id = roleId });*/


            }
            return View(users);

        }
    }
}

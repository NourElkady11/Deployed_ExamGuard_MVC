using AutoMapper;
using Data.Data;
using Data.Models;
using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using DataAccess_Layer.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.Utilities;
using Presentation_Layer.ViewModels;
using System.Security.Claims;

namespace Presentation_Layer.Controllers
{
    public class AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, IMapper mapper) : Controller
    {

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(registerViewModel.Phone, @"^(010|011|012|015)\d{8}$"))
            {
                ModelState.AddModelError("Phone", "Invalid phone number format.");
                return View(registerViewModel);
            }
            else
            {
                var FindUser = await userManager.FindByEmailAsync(registerViewModel.Email);
                if (FindUser is null)
                {

                    var user = new ApplicationUser
                    {
                        Firstname = registerViewModel.FirstName,
                        Lastname=registerViewModel.LastName,
                        UserName = registerViewModel.Username,
                        Email = registerViewModel.Email
                    };


                    HttpContext.Session.SetString("UserEmail", registerViewModel.Email);

                    var result =await userManager.CreateAsync(user, registerViewModel.Password);
                    var studentViewModel = new StudentViewModel()
                    {
                        Name = registerViewModel.Username,
                        Email=registerViewModel.Email,
                        phone=registerViewModel.Phone,
                        address=registerViewModel.address,
                    };

					if(registerViewModel.Image is not null)
					{
                        string imagePath = await DocumentSetting.uploadFile(registerViewModel.Image, "Images");
                        studentViewModel.ImageName = imagePath;

                        // ✅ Store in session
                        HttpContext.Session.SetString("UserImage", imagePath);
                    }

                     var student=mapper.Map<Student>(studentViewModel);
                     await unitOfWork.StudentsRepo.CreateAsync(student);
                     await unitOfWork.SaveChangesAsync();
                      


					if (result.Succeeded)
                    {
                        var roleResult = await userManager.AddToRoleAsync(user,"Student");

                        if (roleResult.Succeeded)
                        {
                            signInManager.SignInAsync(user, isPersistent: true);
                            return RedirectToAction(nameof(Login));
                        }
                        else
                        {
                            foreach (var error in roleResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "This email is already in use.");
                }

                return View(registerViewModel);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
          
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);

            }
            else
            {
             
                var user = userManager.FindByEmailAsync(loginViewModel.Email).Result;
                if (user is not null)
                {
                    if (userManager.CheckPasswordAsync(user, loginViewModel.Password).Result)
                    {
                        var result = signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false).Result;
                        if (result.Succeeded)
                        {
                            HttpContext.Session.SetString("UserEmail", loginViewModel.Email);
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.Firstname) 
                            };


                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);
                            signInManager.Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                            var superVisor = unitOfWork.SuperVisorRepository.GetSuperVisorWithEmail(user.Email).Result;
                            var student = unitOfWork.StudentsRepo.GetStudentWithEmail(user.Email).Result;
                            if (student?.ImageName is not null)
                            {
                                HttpContext.Session.SetString("UserImage", student.ImageName);
                            }
                            if (superVisor?.ImageName is not null)
                            {
                                HttpContext.Session.SetString("UserImage", superVisor.ImageName);
                            }
                            if (loginViewModel.Email == "nourel2ady11@gmail.com")
                            {
                                HttpContext.Session.SetString("UserImage","ADMIN.jpg");
                            }
                            TempData["ShowWelcome"] = "true";
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Incorrect Email or Password");


                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "This email Dosent exist");

                }
            }




            return View(loginViewModel);
        }


        public IActionResult SignOut()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult ForgetPassword()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ForgetPassword(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(forgetPasswordViewModel);

            }
            else
            {
                var user = userManager.FindByEmailAsync(forgetPasswordViewModel.Email).Result;
                if (user is not null)
                {
                
                    var token = userManager.GeneratePasswordResetTokenAsync(user).Result;
                 
                    var url = Url.Action(nameof(ResetPassword), "Account", new { email = forgetPasswordViewModel.Email, Token = token }, Request.Scheme);
                
                    var email = new Mail()
                    {
                        Subject = "Reset your Password",
                        body = url,
                        Recipent = forgetPasswordViewModel.Email
                    };
                    MailSettings.SendEmail(email);

                    return RedirectToAction(nameof(CheckYourInbox));


                }
                else
                {
                    ModelState.AddModelError("Email", "This email Dosent exist");
                }

            }
            return View(forgetPasswordViewModel);

        }

        public IActionResult ResetPassword(string email, string token)
        {
            var stringToken = token?.ToString() ?? string.Empty;
            var stringemail = email?.ToString() ?? string.Empty;
            if (email is null || token is null)
            {
                return BadRequest();
            }
            else
            {
                TempData["stringemail"] = email;
                TempData["stringToken"] = token;

            }

            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            resetPasswordViewModel.token = TempData["stringToken"]?.ToString();
            resetPasswordViewModel.email = TempData["stringemail"]?.ToString();
            if (!ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }
            else
            {
                var user = userManager.FindByEmailAsync(resetPasswordViewModel.email).Result;
                if (user != null)
                {
                    var result = userManager.ResetPasswordAsync(user, resetPasswordViewModel.token, resetPasswordViewModel.Password).Result;
                    if (result.Succeeded)
                    {

                        return RedirectToAction(nameof(Login));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User Not Found");
                }
                return View();
            }
        }

   /*     public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claims => new
            {
                claims.Issuer,
                claims.OriginalIssuer,
                claims.Type,
                claims.Value
            });
            return RedirectToAction("Index", "Home");
        }*/


        public IActionResult CheckYourInbox() => View();





    }



}

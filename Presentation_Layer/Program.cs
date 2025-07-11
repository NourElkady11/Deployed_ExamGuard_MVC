﻿using Data.Data;
using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using DataAccess_Layer.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Presentation_Layer.Middleware;
using Services;
using Services.Abstraction;
using System.Reflection;

namespace Presentation_Layer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<DataIdentityContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("BusinessConnection"));
            });
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddAutoMapper(typeof(Reference).Assembly);
 
            builder.Services.AddSession(); 
            builder.Services.AddHttpContextAccessor(); 

            builder.Services.AddHttpClient("MyHttpClient", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(60);  
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
/*            builder.Services.AddScoped<IExamService, ExamService>();
            builder.Services.AddScoped<IServiceManger, ServiceManger>();*/
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DataIdentityContext>().AddDefaultTokenProviders();


            //AddDefaultTokenProviders==> is for providing the tokens for ForgetPassword and add the defult asccces denied path in the authorization proccess
            builder.Services.AddAuthentication(option =>
            {
          /*      option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;*/
            }).AddGoogle(options =>
            {
                IConfiguration GoogleAuth = builder.Configuration.GetSection("Authentication:Google");
                options.ClientId = GoogleAuth["ClientId"];
                options.ClientSecret = GoogleAuth["ClientSecret"];
            });
/*            }).AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParametershttps://localhost:44378/Files/Images/Cheating-in-Exams.jpg
                {https://localhost:44378/Files/Images/Cheating-in-Exams.jpg
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });*/


            /*    builder.Services.ConfigureApplicationCookie(options =>
				{
					options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
					options.SlidingExpiration = true;
					options.Cookie.HttpOnly = true; 
					options.Cookie.SecurePolicy = CookieSecurePolicy.None; 
					options.LoginPath = "/Account/Login";
					options.AccessDeniedPath = "/Account/AccessDenied";
					// ✅ These settings ensure cookies persist
					options.Cookie.SameSite = SameSiteMode.Lax; // Allows authentication across pages
					options.Cookie.IsEssential = true; // Ensures authentication cookies are always sent
				});*/


            builder.Services.ConfigureApplicationCookie(options =>
			{
				options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
				options.SlidingExpiration = true;
				options.Cookie.HttpOnly = true;
				options.Cookie.SecurePolicy = CookieSecurePolicy.None;
				options.LoginPath = "/Account/Login";
				options.AccessDeniedPath = "/Account/AccessDenied";
				options.Cookie.SameSite = SameSiteMode.Lax;
				options.Cookie.IsEssential = true;
			});


			var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAuthenticationStateMiddleware();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Welcome}/{id?}");

            app.Run();
        }
    }
}

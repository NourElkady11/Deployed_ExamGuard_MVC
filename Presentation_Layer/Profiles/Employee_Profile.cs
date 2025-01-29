using AutoMapper;
using Data.Models;
using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.Profiles
{
    public class Employee_Profile:Profile
    {
        public Employee_Profile() {

            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
        }
    }
}

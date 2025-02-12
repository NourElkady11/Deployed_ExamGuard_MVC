using AutoMapper;
using Data.Models;
using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.Profiles
{
    public class Student_Profile:Profile
    {
        public Student_Profile() {

            CreateMap<Student, StudentViewModel>().ReverseMap();
        }
    }
}

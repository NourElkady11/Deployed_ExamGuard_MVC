using Data.Models;
using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DataAccess_Layer.Repositories
{
    public interface ICourseRepositorys : IGenaricRepository<Course>
    {
        public Task<Course> GetCourseWithExamAsync(int id);
        public Task<List<Course>> GetCourseWithExamssAsync();
        public Task<Course> GetCourseWithNameAsync(string name);
        public Task <List<Course>> GetCourseWithSuperVisorssAsync();
    }
}


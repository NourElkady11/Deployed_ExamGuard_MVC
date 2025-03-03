using Data.Models;
using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using Microsoft.Identity.Client;

namespace DataAccess_Layer.Repositories
{
    public interface ICourseRepositorys : IGenaricRepository<Course>
    {
        public Task<Course> GetCourseWithExamsAsync(int id);
        public Task <List<Course>> GetCourseWithSuperVisorssAsync();
    }
}
using Data.Models;
using DataAccess_Layer.Data;
using DataAccess_Layer.Models;

namespace DataAccess_Layer.Repositories
{
    public interface IStudentRepoistory : IGenaricRepository<Student>
    {
        public Task<IEnumerable<Student>> GetStudentWithNameAsync(string name);


        public Task<IEnumerable<Student>> GetAllStudentsAsync();
    }
}

using DataAccess_Layer.Repositories;
using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using Data.Data;

namespace DataAccess_Layer.Repositories
{
    public class StudentRepository : GenricRepository<Student>, IStudentRepoistory
    {


        public StudentRepository(DataContext dbContext) : base(dbContext)
        {

        }


        public async Task<IEnumerable<Student>> GetStudentWithNameAsync(string name)
        {
            return await dbset.Where(e => e.Name.ToLower().Contains(name.ToLower())).Include(e=>e.CourseStudents).ThenInclude(c => c.Course).ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync() => await dbset.Include(e => e.CourseStudents).ThenInclude(c => c.Course).ToListAsync();

        public async Task<Student> GetStudentWithEmail(string email) => await dbset.Include(e => e.CourseStudents).ThenInclude(c => c.Course).Where(e => e.Email == email).FirstOrDefaultAsync();
    }

}
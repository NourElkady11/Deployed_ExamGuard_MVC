using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Repositories
{
    public interface IUnitOfWork
    {
        public IStudentRepoistory StudentsRepo { get; }
        public ICourseRepositorys CoursesRepo { get; }
        public Task <int> SaveChangesAsync();
    }
}

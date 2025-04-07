using Repositories.Abstractions;
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
        public ISuperVisorRepository SuperVisorRepository { get; }
        public IExamRepository ExamRepository { get; }
        public IStudentExamRepository StudentExamRepository { get; }
        public Task <int> SaveChangesAsync();
    }
}

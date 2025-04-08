using Data.Models;
using DataAccess_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Abstractions
{
    public interface IStudentExamRepository:IGenaricRepository<StudentExam>
    {
        Task<StudentExam> GetAsync(int studentId, int examId);
        Task<IEnumerable<StudentExam>> GetStudentExamsAsync(int studentId);
    }
}

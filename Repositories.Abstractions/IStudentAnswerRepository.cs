using Data.Models;
using DataAccess_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Abstractions
{
    public interface IStudentAnswerRepository:IGenaricRepository<StudentAnswer>
    {
        public Task<List<StudentAnswer>> GetStudentExamAnswersAsync(int studentId, int examId);

       /* Task DeleteStudentExamAnswersAsync(int studentId, int examId);*/
    }
}

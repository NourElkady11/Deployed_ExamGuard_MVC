using Data.Data;
using Data.Models;
using DataAccess_Layer.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class StudentAnswerRepository : GenricRepository<StudentAnswer>, IStudentAnswerRepository
    {
        public StudentAnswerRepository(DataContext context) : base(context)
        {
           
        }

        public async Task<List<StudentAnswer>> GetStudentExamAnswersAsync(int studentId, int examId)
        {
            return await dbset
                .Where(sa => sa.StudentId == studentId && sa.ExamId == examId)
                .ToListAsync();
        }

  /*      public async Task DeleteStudentExamAnswersAsync(int studentId, int examId)
        {
            var answers = await dbset
                .Where(sa => sa.StudentId == studentId && sa.ExamId == examId)
                .ToListAsync();

            dbset.RemoveRange(answers);
        }*/

    }
}

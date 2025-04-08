using Data.Data;
using Data.Migrations.Data;
using Data.Models;
using DataAccess_Layer.Models;
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
    public class StudentExamRepository : GenricRepository<StudentExam>, IStudentExamRepository
    {
        public StudentExamRepository(DataContext context) : base(context)
        {

        }


        public async Task<StudentExam> GetAsync(int studentId, int examId)
        {
            return await dbset.FirstOrDefaultAsync(se => se.StudentId == studentId && se.ExamId == examId);
        }

        public async Task<IEnumerable<StudentExam>> GetStudentExamsAsync(int studentId)
        {
            return await dbset
                .Where(se => se.StudentId == studentId)
                .Include(se => se.exam)
                .ToListAsync();
        }
    }
}

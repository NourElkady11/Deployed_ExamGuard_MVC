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
    public class CheatingReportRepository : GenricRepository<CheatingReport>, ICheatingReportRepository
    {
        public CheatingReportRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CheatingReport>> GetReportsByExamIdAsync(int examId)
        {
            return await dbset
                .Where(r => r.ExamId == examId)
                .Include(r => r.Student)
                .OrderBy(r => r.StudentId)
                .ThenBy(r => r.DetectionTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<CheatingReport>> GetReportsByStudentIdAsync(int studentId)
        {
            return await dbset
                .Where(r => r.StudentId == studentId)
                .Include(r => r.Exam)
                .OrderBy(r => r.ExamId)
                .ThenBy(r => r.DetectionTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<CheatingReport>> GetReportsByExamAndStudentAsync(int examId, int studentId)
        {
            return await dbset
                .Where(r => r.ExamId == examId && r.StudentId == studentId)
                .OrderBy(r => r.DetectionTime)
                .ToListAsync();
        }

    }
}

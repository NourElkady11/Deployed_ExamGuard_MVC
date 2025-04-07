using Data.Data;
using Data.Models;
using DataAccess_Layer.Models;
using DataAccess_Layer.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class ExamRepository : GenricRepository<Exam>, IExamRepository
    {
        private readonly DataContext _context;

        public ExamRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Exam> GetExamWithQuestionsAndChoicesAsync(int id)
        {
            return await _context.Exams
                 .Include(e => e.studentExams)
                .Include(e => e.Course)
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Exam>> GetCourseExamsAsync(int courseid)
        {
            return await _context.Exams
                .Include(e=>e.studentExams)
                .Include(e => e.Course)
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Choices)
                .Where(e => e.CourseId == courseid).ToListAsync();
        }


    }
}
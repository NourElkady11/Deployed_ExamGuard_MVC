using Data.Data;
using Data.Models;
using DataAccess_Layer.Data;
using DataAccess_Layer.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CourseRepository : GenricRepository<Course>, ICourseRepositorys
    {
        private readonly DataContext dataContext;

        public CourseRepository(DataContext context) : base(context)
        {
            this.dataContext = context;
        }
        public async Task<List<Course>> GetCourseWithSuperVisorssAsync()
        {
            return await dbset.Include(e => e.superVisor).ToListAsync();
        }

        public async Task<List<Course>> GetSuperVisorCourses(int id)
        {
            return await dbset.Include(e => e.Exams).Include(e => e.superVisor).Where(e => e.SuperVisorId == id).ToListAsync();
        }

        public async Task<List<Course>> GetCourseWithExamssAsync()
        {
            return await dbset.Include(e => e.Exams).ToListAsync();
        }

        public async Task<Course> GetCourseWithExamAsync(int id)
        {
            return await dbset.Include(e => e.Exams).Include(e=>e.superVisor).Where(e => e.Id == id).FirstOrDefaultAsync();
        
        }

        public async Task<Course> GetCourseWithNameAsync(string name)
        {
            return await dbset.Where(e => e.Name.ToLower().Contains(name.ToLower())).FirstOrDefaultAsync();
        }




     


    }
}

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

        }

        public async Task<Course> GetCourseWithNameAsync(string name)
        {
            return await dbset.Where(e => e.Name.ToLower().Contains(name.ToLower())).FirstOrDefaultAsync();
        }

      
    }
}

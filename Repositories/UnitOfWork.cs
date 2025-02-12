using DataAccess_Layer.Repositories;
using DataAccess_Layer.Data;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories;
using Data.Data;

namespace DataAccess_Layer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Lazy<IStudentRepoistory> studentRepoistory;
        private readonly Lazy<ICourseRepositorys> courseRepositorys;
        private readonly DataContext dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            this.dataContext = dataContext;
            studentRepoistory = new Lazy<IStudentRepoistory>(() => new StudentRepository(dataContext));
            courseRepositorys = new Lazy<ICourseRepositorys>(() => new CourseRepository(dataContext));
        }

        public IStudentRepoistory StudentsRepo => studentRepoistory.Value;

        public ICourseRepositorys CoursesRepo => courseRepositorys.Value;

        public async Task<int> SaveChangesAsync() => await dataContext.SaveChangesAsync();


    }
}

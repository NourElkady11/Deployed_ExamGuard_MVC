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
using Repositories.Abstractions;

namespace DataAccess_Layer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Lazy<IStudentRepoistory> studentRepoistory;
        private readonly Lazy<ICourseRepositorys> courseRepositorys;
        private readonly Lazy<ISuperVisorRepository> superVisorRepository;
        private readonly DataContext dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            this.dataContext = dataContext;
            studentRepoistory = new Lazy<IStudentRepoistory>(() => new StudentRepository(dataContext));
            courseRepositorys = new Lazy<ICourseRepositorys>(() => new CourseRepository(dataContext));
            superVisorRepository = new Lazy<ISuperVisorRepository>(() => new SuperVisorRepository(dataContext));
        }

        public IStudentRepoistory StudentsRepo => studentRepoistory.Value;

        public ICourseRepositorys CoursesRepo => courseRepositorys.Value;

        public ISuperVisorRepository SuperVisorRepository => superVisorRepository.Value;

        public async Task<int> SaveChangesAsync() => await dataContext.SaveChangesAsync();


    }
}

using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusniussLogic_Layer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Lazy<IEmployeeRepoistory> employeeRepoistory;
        private readonly Lazy<IDepartmentRepositorys> departmentRepoistory;
        private readonly DataContext dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            employeeRepoistory=new Lazy<IEmployeeRepoistory>(()=>new EmployeeRepository(dataContext));
            departmentRepoistory=new Lazy<IDepartmentRepositorys>(()=>new DepartmentRepositorys(dataContext));
            this.dataContext = dataContext;
        }

        public IEmployeeRepoistory Employees => employeeRepoistory.Value;


        public IDepartmentRepositorys Departments=> departmentRepoistory.Value;


        public async Task<int> SaveChangesAsync()=>await dataContext.SaveChangesAsync();

		
	}
}

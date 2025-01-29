using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Repositories
{
    public interface IUnitOfWork
    {
        public IEmployeeRepoistory Employees { get; }
        public IDepartmentRepositorys Departments { get; }

        public Task <int> SaveChangesAsync();
    }
}

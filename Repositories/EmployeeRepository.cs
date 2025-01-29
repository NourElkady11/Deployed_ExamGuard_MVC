using DataAccess_Layer.Repositories;
using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace DataAccess_Layer.Repositories
{
    public class EmployeeRepository : GenricRepository<Employee>, IEmployeeRepoistory
    {


        public EmployeeRepository(DataIdentityContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(string name)
        {
            return await dbset.Where(e => e.Name.ToLower().Contains(name.ToLower())).Include(d => d.Department).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllWithDepartmentAsync() => await dbset.Include(e => e.Department).ToListAsync();
    }

}

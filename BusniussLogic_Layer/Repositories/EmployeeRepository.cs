
using Microsoft.EntityFrameworkCore;

namespace BusniussLogic_Layer.Repositories
{
    public class EmployeeRepository : GenricRepository<Employee>, IEmployeeRepoistory
    {
      

        public EmployeeRepository(DataContext dbContext):base(dbContext)
        {
          
        }

        public async Task< IEnumerable<Employee>> GetAllEmployeesAsync(string name)
        {
            return await dbset.Where(e => e.Name.ToLower().Contains(name.ToLower())).Include(d=>d.Department).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllWithDepartmentAsync()=> await dbset.Include(e => e.Department).ToListAsync();
    }
}

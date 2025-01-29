using Data.Models;
using DataAccess_Layer.Data;
using DataAccess_Layer.Models;

namespace DataAccess_Layer.Repositories
{
    public interface IEmployeeRepoistory : IGenaricRepository<Employee>
    {
        public Task<IEnumerable<Employee>> GetAllEmployeesAsync(string name);


        public Task<IEnumerable<Employee>> GetAllWithDepartmentAsync();
    }
}

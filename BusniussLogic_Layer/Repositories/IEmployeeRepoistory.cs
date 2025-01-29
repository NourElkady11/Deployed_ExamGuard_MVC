
namespace BusniussLogic_Layer.Repositories
{
    public interface IEmployeeRepoistory:IGenaricRepository<Employee>
    {
        public Task<IEnumerable<Employee>> GetAllEmployeesAsync(string name);


        public Task< IEnumerable<Employee>> GetAllWithDepartmentAsync();
    }
}

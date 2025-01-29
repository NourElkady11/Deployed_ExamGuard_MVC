
namespace BusniussLogic_Layer.Repositories
{
    public class DepartmentRepositorys : GenricRepository<Department>,IDepartmentRepositorys
    {
        private readonly DataContext dataContext;

        public DepartmentRepositorys(DataContext context) : base(context)
        {

        }
        /////implement any added repos
    
    }
}

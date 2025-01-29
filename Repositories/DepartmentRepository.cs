using Data.Models;
using DataAccess_Layer.Data;
using DataAccess_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class DepartmentRepository : GenricRepository<Department>, IDepartmentRepositorys
    {
        private readonly DataIdentityContext dataContext;

        public DepartmentRepository(DataIdentityContext context) : base(context)
        {

        }
    }
}

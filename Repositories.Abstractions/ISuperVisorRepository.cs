using Data.Models;
using DataAccess_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Abstractions
{
    public interface ISuperVisorRepository : IGenaricRepository<SuperVisor>
    {
        public Task<SuperVisor?> GetSuperVisorWithEmail(string email);
    }
}

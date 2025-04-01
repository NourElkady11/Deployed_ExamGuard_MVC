using Data.Data;
using Data.Models;
using DataAccess_Layer.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class SuperVisorRepository : GenricRepository<SuperVisor>, ISuperVisorRepository
    {
        public SuperVisorRepository(DataContext context) : base(context)
        {
        }

        public async Task<SuperVisor?> GetSuperVisorWithEmail(string email) => await dbset.Where(e => e.Email == email).FirstOrDefaultAsync();

        
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusniussLogic_Layer.Repositories
{
    public class GenricRepository<Tentity> : IGenaricRepository<Tentity> where Tentity : class
    {
        protected DataContext context;
        protected DbSet<Tentity> dbset;
        

        public GenricRepository(DataContext context)
        {
            this.context = context;
            dbset=context.Set<Tentity>();
        }

        public async Task CreateAsync(Tentity entity)
        {
           await dbset.AddAsync(entity);
        }

        public void Delete(Tentity entity)
        {
            dbset.Remove(entity);
        }

        public async Task <Tentity?> GetAsync(int id) => await dbset.FindAsync(id);
        public async Task<IEnumerable<Tentity>> GetAllAsync()=>await dbset.ToListAsync();
       

        public void Update(Tentity entity)
        {
            dbset.Update(entity);
        }

	
	}
}

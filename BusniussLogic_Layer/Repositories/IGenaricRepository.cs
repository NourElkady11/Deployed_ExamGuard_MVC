using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusniussLogic_Layer.Repositories
{
    public interface IGenaricRepository<TEntity> where TEntity: class
    {
        Task CreateAsync(TEntity entity);
        void Delete(TEntity entity);
        Task<TEntity?> GetAsync(int id);
        Task<IEnumerable<TEntity>>? GetAllAsync();
        void Update(TEntity entity);
    }
}

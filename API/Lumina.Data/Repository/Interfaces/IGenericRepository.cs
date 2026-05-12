using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Repository.Interfaces;

public  interface IGenericRepository<TEntity,Tkey>
{

    IQueryable<TEntity> GetAllAttached();

    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task AddAsync(TEntity entity);

    Task Delete(TEntity entity);

    Task SaveChangesAsync();

    Task<TEntity> GetByIdAsync(Tkey id);
}

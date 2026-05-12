using Lumina.Data.Repository.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Lumina.Data.Repository;


public abstract class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey>
    where TEntity : class
{
    protected readonly CinemaAppDBContext _cinemaDbContext;
    protected readonly DbSet<TEntity> _dbSet;
    protected GenericRepository(CinemaAppDBContext cinemaDbContext)
    {
        this._cinemaDbContext = cinemaDbContext;
        this._dbSet = _cinemaDbContext.Set<TEntity>();

    }


    public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return this._dbSet.FirstOrDefaultAsync(predicate);
    }

    public virtual IQueryable<TEntity> GetAllAttached()
    {
        return this._dbSet.AsQueryable();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await this._dbSet.ToListAsync();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await this._dbSet.AddAsync(entity);
        await this._cinemaDbContext.SaveChangesAsync();
    }

    public virtual async Task Delete(TEntity entity)
    {
        this._dbSet.Remove(entity);
        await this._cinemaDbContext.SaveChangesAsync();
    }

    public virtual async Task SaveChangesAsync()
    {
        try
        {
            await this._cinemaDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.ToString());
        }
    }

    public async Task<TEntity> GetByIdAsync(Tkey id)
    {
        return await this._dbSet.FindAsync(id);
    }

}


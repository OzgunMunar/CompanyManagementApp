using AspNetCoreMVCApp.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMVCApp.Repositories
{

    public interface IRepositoryBase<T> where T: class
    {
        Task Create(T entity);
        Task Edit(T entity);
        Task DeletePermanently(T entity);
        Task Delete(T entity);
        
    }
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {

        private readonly MyCompanyDbContext _dbContext;
        private DbSet<T> _dbSet;

        public RepositoryBase(MyCompanyDbContext dbContext)
        {
            _dbContext = dbContext;
            this._dbSet = _dbContext.Set<T>();
        }

        public async Task Create(T entity) 
        {
            if(entity is null)
                throw new ArgumentNullException("entity");

            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Edit(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException("entity");

            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException("entity");

            await Edit(entity);
        }

        public async Task DeletePermanently(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException("entity");

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}

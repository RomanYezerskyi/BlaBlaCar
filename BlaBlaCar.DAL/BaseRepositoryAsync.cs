using System.Linq.Expressions;
using BlaBlaCar.DAL.Data;
using BlaBlaCar.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BlaBlaCar.DAL
{
    public class BaseRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
    {
        private ApplicationDbContext _context;
        private DbSet<TEntity> dbSet;
        public BaseRepositoryAsync(ApplicationDbContext context)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> GetAsync(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null, 
            Expression<Func<TEntity, bool>> filter = null, 
            int skip = 0, 
            int take = int.MaxValue)
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();

            queryable = skip == 0? queryable.Take(take) : queryable.Skip(skip).Take(take);
            queryable = filter is null ? queryable : queryable.Where(filter);
            queryable = includes is null ? queryable : includes(queryable);
            queryable = orderBy is null ? queryable : orderBy(queryable);

            
            //if (includes != null)
            //{
            //    queryable = includes(queryable);
            //}

            //if (filter != null)
            //{

            //    queryable = queryable.Where(filter);
            //}
            //if (orderBy != null)
            //{
            //    return await orderBy(queryable).ToListAsync();
            //}

            return await queryable.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();
            if (includes != null)
            {
                queryable = includes(queryable);
            }

            if (filter != null)
            {
                queryable = queryable.Where(filter);
            }

            return await queryable.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }
        public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }
        public void Update(TEntity entityToUpdate)
        {
            _context.Set<TEntity>().Update(entityToUpdate);
        }

        public void Delete(TEntity entityToDelete)
        {
            _context.Set<TEntity>().Remove(entityToDelete);
        }

        public void Delete(IEnumerable<TEntity> entityToDelete)
        {
            _context.Set<TEntity>().RemoveRange(entityToDelete);
        }
    }
}

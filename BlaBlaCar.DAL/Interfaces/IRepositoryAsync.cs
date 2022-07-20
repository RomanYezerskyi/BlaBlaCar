﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace BlaBlaCar.DAL.Interfaces
{
    public interface IRepositoryAsync<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Expression<Func<TEntity, bool>> filter = null);
        //Task<IEnumerable<TEntity>> GetAsync(int? page, int limit, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        //    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
        //    Expression<Func<TEntity, bool>> filter = null);
        Task<TEntity> GetAsync(
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Expression<Func<TEntity, bool>> filter = null);
        //Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);
        Task InsertAsync(TEntity entity);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        void Update(TEntity entityToUpdate);
        void Delete(TEntity entityToDelete);
        void Delete(IEnumerable<TEntity> entityToDelete);
    }
}

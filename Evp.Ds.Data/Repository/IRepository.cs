using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Evp.Ds.Data.Repository
{
    /// <summary>
    ///     Abstract persistance layer to be framework agnostic.
    ///     Any ORM frameworks just need to implement this interface.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> Table { get; }
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
        void Delete(TEntity entity);
        void CreateOrUpdate(TEntity entity);
        void Save();
        void SaveChangeWithoutValidation();
        Task SaveAsync();

        TEntity Get(int id);
        TEntity Get(int id, params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<TEntity> GetAsync(int id);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

        int Count(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeExpressions);

        IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> predicate, Action<Orderable<TEntity>> order);

        IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> predicate, Action<Orderable<TEntity>> order, int skip,
            int count);

        IEnumerable<TModel> Fetch<TModel>(Expression<Func<TEntity, TModel>> columns) where TModel : class;

        IEnumerable<TModel> Fetch<TModel>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TModel>> columns) where TModel : class;

        IEnumerable<TModel> Fetch<TModel>(Expression<Func<TEntity, bool>> predicate, Action<Orderable<TEntity>> order,
            Expression<Func<TEntity, TModel>> columns) where TModel : class;
    }
}

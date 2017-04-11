using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Evp.Ds.Data.Core;
using Evp.Ds.Data.Extension;

namespace Evp.Ds.Data.Repository
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<T> _dbSet;

        public EFRepository(IDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        }

        public virtual T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual T Get(int id, params Expression<Func<T, object>>[] includeExpressions)
        {
            if (!includeExpressions.Any())
                return Get(id);

            foreach (var expression in includeExpressions)
                _dbSet.Include(expression);

            return _dbSet.Find(id);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            var dbset = _dbSet as DbSet<T>;
            return await dbset.FindAsync(id);
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate).SingleOrDefault();
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Fetch(predicate).SingleOrDefaultAsync();
        }

        public virtual void Create(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            object existing;

            var objContext = ((IObjectContextAdapter) _dbContext).ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            if (objContext.TryGetObjectByKey(entityKey, out existing))
                _dbContext.Entry(existing).State = EntityState.Detached;

            _dbContext.Set<T>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            object existing;

            var objContext = ((IObjectContextAdapter) _dbContext).ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            if (objContext.TryGetObjectByKey(entityKey, out existing))
                _dbContext.Entry(existing).State = EntityState.Detached;

            _dbContext.Set<T>().Attach(entity);
            foreach (var prop in properties)
                _dbContext.Entry(entity).Property(prop).IsModified = true;
        }

        public virtual void Delete(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }

        public virtual void Save()
        {
            _dbContext.SaveChanges();
        }

        public virtual async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate).Count();
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order)
        {
            var orderable = new Orderable<T>(Fetch(predicate));
            order(orderable);
            return orderable.Queryable;
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip,
            int count)
        {
            return Fetch(predicate, order).Skip(skip).Take(count);
        }

        #region IRepository<T> Members

        void IRepository<T>.Create(T entity)
        {
            Create(entity);
        }

        void IRepository<T>.Update(T entity)
        {
            Update(entity);
        }

        void IRepository<T>.Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            Update(entity, properties);
        }

        void IRepository<T>.Delete(T entity)
        {
            Delete(entity);
        }

        void IRepository<T>.Save()
        {
            Save();
        }

        void IRepository<T>.SaveChangeWithoutValidation()
        {
            _dbContext.Configuration.ValidateOnSaveEnabled = false;
            Save();
            _dbContext.Configuration.ValidateOnSaveEnabled = true;
        }

        async Task IRepository<T>.SaveAsync()
        {
            await SaveAsync();
        }

        void IRepository<T>.CreateOrUpdate(T entity)
        {
            _dbSet.AddOrUpdate(entity);
        }

        T IRepository<T>.Get(int id)
        {
            return Get(id);
        }

        T IRepository<T>.Get(int id, params Expression<Func<T, object>>[] includeExpressions)
        {
            return Get(id, includeExpressions);
        }

        Task<T> IRepository<T>.GetAsync(int id)
        {
            return GetAsync(id);
        }

        T IRepository<T>.Get(Expression<Func<T, bool>> predicate)
        {
            return Get(predicate);
        }

        Task<T> IRepository<T>.GetAsync(Expression<Func<T, bool>> predicate)
        {
            return GetAsync(predicate);
        }

        IQueryable<T> IRepository<T>.Table => _dbSet;

        int IRepository<T>.Count(Expression<Func<T, bool>> predicate)
        {
            return Count(predicate);
        }

        /// <summary>
        ///     Fetch a set of records based on predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Return a read-only IEnumberable collection</returns>
        IEnumerable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate).ToReadOnlyCollection();
        }

        IEnumerable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeExpressions)
        {
            foreach (var expression in includeExpressions)
                _dbSet.Include(expression);

            return _dbSet.Where(predicate).ToList();
        }

        IEnumerable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order)
        {
            return Fetch(predicate, order).ToReadOnlyCollection();
        }

        IEnumerable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip,
            int count)
        {
            return Fetch(predicate, order, skip, count).ToReadOnlyCollection();
        }

        IEnumerable<TModel> IRepository<T>.Fetch<TModel>(Expression<Func<T, TModel>> columns)
        {
            return _dbSet.Select(columns);
        }

        IEnumerable<TModel> IRepository<T>.Fetch<TModel>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TModel>> columns)
        {
            return Fetch(predicate).Select(columns).ToReadOnlyCollection();
        }

        IEnumerable<TModel> IRepository<T>.Fetch<TModel>(Expression<Func<T, bool>> predicate,
            Action<Orderable<T>> order,
            Expression<Func<T, TModel>> columns)
        {
            return Fetch(predicate, order).Select(columns).ToReadOnlyCollection();
        }

        #endregion
    }
}
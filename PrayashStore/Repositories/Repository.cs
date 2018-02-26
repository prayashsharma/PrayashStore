using PrayashStore.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PrayashStore.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        private DbSet<TEntity> _dbset;

        public Repository(DbContext context)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }

        public TEntity Get(int id)
        {
            return _dbset.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbset.ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbset.Where(predicate).ToList();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbset.SingleOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            _dbset.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbset.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbset.RemoveRange(entities);
        }

        public void Edit(TEntity entity, int key)
        {
            var existing = _dbset.Find(key);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
            }
        }

        public IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbset;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.AsEnumerable();
        }
    }
}
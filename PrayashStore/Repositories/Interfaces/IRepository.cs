﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PrayashStore.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        void Edit(TEntity entity, int key);

        IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties);
    }
}

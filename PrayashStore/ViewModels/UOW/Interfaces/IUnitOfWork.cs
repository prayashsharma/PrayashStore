using PrayashStore.Repositories.Interfaces;
using System;

namespace PrayashStore.UOW.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        int Complete();
    }
}

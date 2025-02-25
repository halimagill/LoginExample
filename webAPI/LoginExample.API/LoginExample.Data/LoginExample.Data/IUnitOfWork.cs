using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginExample.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Dictionary<string, dynamic> Repositories { get; set; }
        T Repository<T>() where T : class;
        //public IRepository<T> Repository<T>() where T : class;
        //    IRepository<T> GetRepository<TEntity>();
        //public bool SaveChanges();
        Task SaveChangesAsync();
        Task CompleteAsync();

        /// <summary>
        /// Discards all changes that has not been commited
        /// </summary>
        void RejectChanges();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : IEfCoreDbContext
    {
        TContext Context { get; }
    }
}
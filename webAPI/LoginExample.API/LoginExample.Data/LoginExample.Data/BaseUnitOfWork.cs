using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LoginExample.Data
{
    public abstract class BaseUnitOfWork<TContext> where TContext : IEfCoreDbContext
    {
        public Dictionary<string, dynamic> Repositories { get; set; }
        public TContext Context { get; }

        public BaseUnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public T Repository<T>() where T : class
        {
            var result = (T)Activator.CreateInstance(typeof(T), this);

            if (result != null)
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Completes the unit of work, saving all repository changes to the underlying data-store.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        public async Task CompleteAsync() => await SaveChangesAsync();

        public async Task SaveChangesAsync()
        {
            Console.WriteLine($"Invoke SaveChangesAsync in context with hashcode: {Context.GetHashCode()}");
            await Context.SaveChangesAsync();
        }

        #region IDisposable Support  
        private bool _disposedValue = false; // To detect redundant calls  
        public void Dispose()
        {
            Context.Dispose();
        }

        #endregion

        public void RejectChanges()
        {
            foreach (var entry in Context.ChangeTracker.Entries()
                  .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
    }
}

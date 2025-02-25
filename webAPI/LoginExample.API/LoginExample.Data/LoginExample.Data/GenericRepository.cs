using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LoginExample.Data
{
    public class GenericRepository<T> : IRepository<T> where T : class, new()
    {
        internal DbSet<T> dbSet;
        internal IUnitOfWork<IAppIdentityDbContext> _uow;
        private static Dictionary<string, dynamic> _repositories;
        public GenericRepository(IUnitOfWork<IAppIdentityDbContext> unitOfWork)
        {
            _uow = unitOfWork;
            this.dbSet = unitOfWork.Context.Set<T>();
        }

        public virtual async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual async Task<T> Get(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllASync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public virtual IQueryable<T> Queryable()
        {
            return dbSet;
        }

        public T GetRepository()
        {
            var result = _uow.Repository<T>();

            if (result != null)
            {
                return result;
            }

            return null;
        }
        public string GetCommaSeparatedNames(List<string> items)
        {
            string ret = string.Join(", ", items);
            return ret;
        }
    }
}

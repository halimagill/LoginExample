using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginExample.Data
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        Task<T> Get(int id);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllASync();
        Task Add(T entity);
        void Delete(T entity);
        void Update(T entity);

        IQueryable<T> Queryable();

        T GetRepository();

        public string GetCommaSeparatedNames(List<string> items);

    }
}


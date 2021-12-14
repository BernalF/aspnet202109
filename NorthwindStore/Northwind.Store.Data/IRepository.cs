using Northwind.Store.Notification;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Northwind.Store.Data
{
    /// <summary>
    /// CRUD
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TK"></typeparam>
    public interface IRepository<T> : IDisposable
    {
        Task<int> Save(T model, Notifications n = null);
        Task<T> Get(Expression<Func<T, bool>> filter, string includeProperties = "");
        Task<IEnumerable<T>> GetList(int? pageNumber, int pageSize, string includeProperties = "");
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> Find(Expression<Func<T,bool>> predicate);
        Task<int> Delete(T model);
    }
}

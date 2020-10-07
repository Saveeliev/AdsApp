using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdsApp.Services
{
    public interface IDataProvider
    {
        Task Insert<T>(T model) where T : class;
        Task Update<T>(T model) where T : class;
        IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task Delete<T>(T model) where T : class;
    }
}

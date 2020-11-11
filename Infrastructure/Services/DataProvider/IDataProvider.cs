using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Services.DataProvider
{
    public interface IDataProvider
    {
        Task Insert<T>(T model) where T : class;
        Task Update<T>(T model) where T : class;
        IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate = null) where T : class;
        Task Delete<T>(T model) where T : class;
        IDbContextTransaction CreateTransaction(IsolationLevel isolationLevel);
    }
}
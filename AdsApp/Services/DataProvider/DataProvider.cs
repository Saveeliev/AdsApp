using AdsApp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdsApp.Services
{
    public class DataProvider : IDataProvider
    {
        public readonly AdsAppContext _context;

        public DataProvider(AdsAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Insert<T>(T model) where T : class
        {
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task Update<T>(T model) where T : class
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return _context.Set<T>().Where(predicate);
        }

        public async Task Delete<T>(T model) where T : class
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
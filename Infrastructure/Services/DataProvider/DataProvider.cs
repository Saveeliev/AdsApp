﻿using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Services.DataProvider
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

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate = null) where T : class
        {
            var query = _context.Set<T>().AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;
        }

        public async Task Delete<T>(T model) where T : class
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }

        public IDbContextTransaction CreateTransaction(IsolationLevel isolationLevel)
        {
            var transaction = _context.Database.BeginTransaction(isolationLevel);

            return transaction;
        }
    }
}
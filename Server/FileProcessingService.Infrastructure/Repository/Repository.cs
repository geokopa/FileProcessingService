using FileProcessingService.Application.Common.Interfaces.Repository;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace FileProcessingService.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _set;

        public Repository(DbContext context)
        {
            _set = context.Set<T>();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> expression = null)
        {
            if(expression != null)
            {
                return _set.Where(expression).AsQueryable();
            }
            return _set.AsQueryable<T>();
        }

        public async Task<IEnumerable<T>> FetchBy(Expression<Func<T, bool>> predicate)
        {
            return await _set.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _set.AddAsync(entity);
        }

        public async Task<T> FindAsync(int key)
        {
            return await _set.FindAsync(key);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _set.FirstOrDefaultAsync(predicate);
        }

        public async ValueTask<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _set.AnyAsync(predicate);
        }

        public async ValueTask<int> CountAsync()
        {
            return await _set.AsNoTracking().CountAsync();
        }
    }
}

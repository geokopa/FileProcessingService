using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileProcessingService.Application.Common.Interfaces.Repository
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindAsync(int id);
        ValueTask<int> CountAsync();
        ValueTask<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}

using DataTransferObjects.Request.Common;
using DataTransferObjects.Response.Common;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<T?> GetByIdAsync(object id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<DataTableResponseDTO<T>> GetAllAsync(
        DataTableRequestDTO dto,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task PatchAsync(T entity, IEnumerable<string> propertiesToUpdate);
        Task DeleteAsync(object id);
        Task SaveChangesAsync();
    }
}

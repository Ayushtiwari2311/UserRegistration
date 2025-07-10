using Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Domain.RepositoryInterfaces;
using DataTransferObjects.Request.Common;
using DataTransferObjects.Response.Common;
using Infrastructure.Extensions;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.RepositoryImplementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;
        private IDbContextTransaction _transaction;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_transaction != null)
                return _transaction;

            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public virtual async Task<T> GetByIdAsync(object id, 
                                                Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null) 
        {
           IQueryable<T> query = _context.Set<T>();
           if (include != null)
              query = include(query); // supports ThenInclude
           return await query.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id));
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task PatchAsync(T entity, IEnumerable<string> propertiesToUpdate)
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            var entityProperties = entityType.GetProperties().Select(p => p.Name).ToHashSet();
            foreach (var propertyName in propertiesToUpdate)
            {
                if (entityProperties.Contains(propertyName))
                {
                    _context.Entry(entity).Property(propertyName).IsModified = true;
                }
            }
        }

        public virtual async Task DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<DataTableResponseDTO<T>> GetAllAsync(
        DataTableRequestDTO dto,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (include != null)
                query = include(query); // supports ThenInclude

            if (filter != null)
                query = query.Where(filter);

            var totalCountFiltered = await query.CountAsync();

            if (orderBy != null)
                query = orderBy(query);
            else
                query = query.OrderByDescending(e => EF.Property<DateTime>(e, "CreatedOn"));

            var data = await query
                .Skip(dto.Start * dto.Length)
                .Take(dto.Length)
                .ToListAsync();

            var totalCount = await _dbSet.CountAsync();

            return new DataTableResponseDTO<T>
            {
                draw = dto.Draw,
                recordsTotal = totalCount,
                recordsFiltered = totalCountFiltered,
                data = data
            };
        }
    }

}

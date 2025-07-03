using Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Domain.RepositoryInterfaces;
using DataTransferObjects.Request.Common;
using DataTransferObjects.Response.Common;
using Infrastructure.Extensions;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.RepositoryImplementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
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

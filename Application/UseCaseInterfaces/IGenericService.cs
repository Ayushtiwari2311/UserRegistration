using DataTransferObjects.Request.Common;
using DataTransferObjects.Response.Common;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCaseInterfaces
{
    public interface IGenericService<TEntity, TSaveDto, TGetDto>
    where TEntity : class
    {
        Task<APIResponseDTO<DataTableResponseDTO<TGetDto>>> GetAllAsync(
            DataTableRequestDTO dto,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
        Task<APIResponseDTO> AddAsync(TSaveDto dto);
        Task<APIResponseDTO> UpdateAsync(TSaveDto entity);
        Task<APIResponseDTO> DeleteAsync(object id);
        Task<APIResponseDTO<TGetDto>> GetByIdAsync(object id);
        Task<APIResponseDTO> SaveChangesAsync();
    }
}

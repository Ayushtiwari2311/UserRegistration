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
    public interface IGenericService<TEntity, TSaveDto,TUpdateDto, TGetDto>
    where TEntity : class
    {
        Task<APIResponseDTO<DataTableResponseDTO<TGetDto>>> GetAllAsync(
            DataTableRequestDTO dto,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
        Task<APIResponseDTO<TGetDto>> GetByIdAsync(object id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
        Task<APIResponseDTO> AddAsync(TSaveDto dto);
        Task<APIResponseDTO<TGetDto>> AddWithReturnAsync(TSaveDto dto,
                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                    Func<TEntity, object> keySelector = null!);
        Task<APIResponseDTO> UpdateAsync(object id,TUpdateDto entity);
        Task<APIResponseDTO<TGetDto>> UpdateWithReturnAsync(object id, TUpdateDto dto, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
        Task<APIResponseDTO> PatchAsync(object id, Dictionary<string, object> updatedFields);
        Task<APIResponseDTO<TGetDto>> PatchWithReturnAsync(object id, Dictionary<string, object> updatedFields, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
        Task<APIResponseDTO> DeleteAsync(object id);
        Task<APIResponseDTO> SaveChangesAsync();
    }
}

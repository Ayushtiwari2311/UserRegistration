using Application.Helpers.Patch;
using Application.UseCaseInterfaces;
using AutoMapper;
using DataTransferObjects.Request.Common;
using DataTransferObjects.Response.Common;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCaseImplementation
{
    public class GenericService<TEntity, TSaveDto, TUpdateDto, TGetDto>: IGenericService<TEntity, TSaveDto, TUpdateDto, TGetDto>
    where TEntity : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<APIResponseDTO> AddAsync(TSaveDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();   
            return APIResponseDTO.Ok("Record added successfully.");
        }

        public virtual async Task<APIResponseDTO<TGetDto>> AddWithReturnAsync(TSaveDto dto,
                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                    Func<TEntity, object> keySelector = null!)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            var entityKey = keySelector(entity);
            var savedEntity = await _repository.GetByIdAsync(entityKey,include);
            var resultDto = _mapper.Map<TGetDto>(savedEntity);
            return APIResponseDTO<TGetDto>.Ok(resultDto, "Record added successfully.");
        }

        public virtual async Task<APIResponseDTO> UpdateAsync(object id, TUpdateDto dto)
        {
            var dbEntity = await _repository.GetByIdAsync(id);
            if (dbEntity == null)
                return APIResponseDTO.Fail("Record not found.");

            _mapper.Map(dto,dbEntity);
            await _repository.UpdateAsync(dbEntity);
            await _repository.SaveChangesAsync();
            return APIResponseDTO.Ok("Record updated successfully.");
        }

        public virtual async Task<APIResponseDTO<TGetDto>> UpdateWithReturnAsync(object id, TUpdateDto dto
                                        ,Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            var dbEntity = await _repository.GetByIdAsync(id);
            if (dbEntity == null)
                return APIResponseDTO<TGetDto>.Fail("Record not found.");

            _mapper.Map(dto, dbEntity);

            await _repository.UpdateAsync(dbEntity);
            await _repository.SaveChangesAsync();

            var updated = await _repository.GetByIdAsync(id, include);
            var resultDto = _mapper.Map<TGetDto>(updated);
            return APIResponseDTO<TGetDto>.Ok(resultDto, "Record updated successfully.");
        }


        public async Task<APIResponseDTO> PatchAsync(object id, Dictionary<string,object> updatedFields)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return APIResponseDTO.Fail("Record not found.");

            foreach (var kvp in updatedFields)
            {
                var prop = typeof(TEntity).GetProperty(kvp.Key);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(entity, Convert.ChangeType(kvp.Value, prop.PropertyType));
                }
            }

            await _repository.PatchAsync(entity, updatedFields.Keys);
            await _repository.SaveChangesAsync();

            return APIResponseDTO.Ok("Record updated.");
        }

        public virtual async Task<APIResponseDTO<TGetDto>> PatchWithReturnAsync(object id, Dictionary<string, object> updatedFields,
                                                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            var entity = await _repository.GetByIdAsync(id,include);
            if (entity == null)
                return APIResponseDTO<TGetDto>.Fail("Record not found.");

            foreach (var kvp in updatedFields)
            {
                var prop = typeof(TEntity).GetProperty(kvp.Key);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(entity, Convert.ChangeType(kvp.Value, prop.PropertyType));
                }
            }

            await _repository.PatchAsync(entity, updatedFields.Keys);
            await _repository.SaveChangesAsync();

            var resultDto = _mapper.Map<TGetDto>(entity);
            return APIResponseDTO<TGetDto>.Ok(resultDto, "Record updated.");
        }

        public virtual async Task<APIResponseDTO<DataTableResponseDTO<TGetDto>>> GetAllAsync(
            DataTableRequestDTO dto,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            var result = await _repository.GetAllAsync(dto, filter, orderBy, include);

            var responseDto = new DataTableResponseDTO<TGetDto>
            {
                draw = dto.Draw,
                recordsTotal = result.recordsTotal,
                recordsFiltered = result.recordsFiltered,
                data = _mapper.Map<IEnumerable<TGetDto>>(result.data)
            };

            return APIResponseDTO<DataTableResponseDTO<TGetDto>>.Ok(responseDto);
        }

        public virtual async Task<APIResponseDTO<TGetDto>> GetByIdAsync(object id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            var entity = await _repository.GetByIdAsync(id, include);
            if (entity is null)
                return APIResponseDTO<TGetDto>.Fail("Record not found.");

            return APIResponseDTO<TGetDto>.Ok(_mapper.Map<TGetDto>(entity));
        }
        public virtual async Task<APIResponseDTO> DeleteAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return APIResponseDTO.Fail("No Record found.");

            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return APIResponseDTO.Ok("Record deleted.");
        }

        public virtual async Task<APIResponseDTO> SaveChangesAsync()
        {
            await _repository.SaveChangesAsync();
            return APIResponseDTO.Ok("Changes saved.");
        }
    }
}

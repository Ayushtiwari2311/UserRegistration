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
    public class GenericService<TEntity, TSaveDto, TGetDto> : IGenericService<TEntity, TSaveDto, TGetDto>
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

        public virtual async Task<APIResponseDTO> UpdateAsync(TSaveDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
            return APIResponseDTO.Ok("Record updated successfully.");
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

        public virtual async Task<APIResponseDTO<TGetDto>> GetByIdAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id);
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

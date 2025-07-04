using Application.Helpers;
using Application.Helpers.Image;
using Application.Helpers.Patch;
using Application.UseCaseInterfaces;
using AutoMapper;
using DataTransferObjects.Request.Common;
using DataTransferObjects.Request.User;
using DataTransferObjects.Response.Common;
using DataTransferObjects.Response.User;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.UseCaseImplementation
{
    public class UserRegistrationService(
    IUserRegistrationRepository repository,
    IMapper mapper,
    IImageHelper imageHelper,
    IPatchHelper patchHelper
) : GenericService<TrnUserRegistration, SaveUserResgistrationDTO,UpdateUserRegistrationDTO, GetUserResponseDTO>(repository, mapper),
    IUserRegistrationService
    {
        public new async Task<APIResponseDTO> AddAsync(SaveUserResgistrationDTO dto)
        {
            dto.PhotoPath = await imageHelper.SaveImageAsync(dto.Photo, Constants.UserProfileImages);
            if (!await repository.CheckUserExistsByEmail(dto.Email))
            {
                var response = await base.AddAsync(dto);
                if (!response.IsSuccess) { 
                    imageHelper.DeleteImage(Constants.UserProfileImages, dto.PhotoPath);
                }
                return response;
            }
            else
                return APIResponseDTO.Fail($"User already exists with email {dto.Email}!");
        }

        public new async Task<APIResponseDTO<DataTableResponseDTO<GetUserResponseDTO>>> GetAllAsync(GetUserRequestListDTO dto)
        {
            Expression<Func<TrnUserRegistration, bool>>? filter = null;

            if (!string.IsNullOrEmpty(dto.searchValue))
            {
                Expression<Func<TrnUserRegistration, bool>> searchFilter = u => u.Name.Contains(dto.searchValue);
                filter = filter.AndAlso(searchFilter);
            }

            if (dto.StateId.HasValue)
            {
                Expression<Func<TrnUserRegistration, bool>> stateFilter = u => u.StateId == dto.StateId;
                filter = filter.AndAlso(stateFilter);
            }

            if (dto.CityId.HasValue)
            {
                Expression<Func<TrnUserRegistration, bool>> cityFilter = u => u.CityId == dto.CityId;
                filter = filter.AndAlso(cityFilter);
            }

            return await base.GetAllAsync(dto, filter, null, query => query
                                                                    .Include(u => u.Gender)
                                                                    .Include(u => u.State)
                                                                    .Include(u => u.City)
                                                                    .Include(u => u.UserHobbies)
                                                                       .ThenInclude(uh => uh.Hobby));
        }

        public new async Task<APIResponseDTO> DeleteAsync(Guid Id)
        {
            var record = await repository.GetByIdAsync(Id);
            var response = await base.DeleteAsync(Id);
            if(response.IsSuccess && !string.IsNullOrEmpty(record?.PhotoPath))
            {
                await imageHelper.DeleteImage(Constants.UserProfileImages, record.PhotoPath);
            }
            return response;
        }

        public new async Task<APIResponseDTO> UpdateAsync(Guid id, UpdateUserRegistrationDTO dto)
        {
            using var transaction = await repository.BeginTransactionAsync();
            try
            {
                var dbEntity = await repository.GetByIdAsync(id);
                if (dbEntity == null)
                    return APIResponseDTO.Fail("Record not found.");

                dto.PhotoPath = dto.Photo != null
                        ? await imageHelper.UpdateImageAsync(dto.Photo, Constants.UserProfileImages, dbEntity.PhotoPath) :
                         dbEntity.PhotoPath;

                if (dto.Hobbies is { Count: > 0 })
                {
                    var newHobbies = dto.Hobbies.Distinct().Select(hobbyId => new TrnUserHobby
                    {
                        HobbyId = hobbyId,
                        UserId = dbEntity.Id
                    }).ToList();

                    await repository.UpdateHobbies(id, newHobbies);
                }
                var result =  await base.UpdateAsync(id, dto);
                await repository.CommitTransactionAsync();
                return result;
            }
            catch
            {
                await repository.RollbackTransactionAsync();
                throw;
            }
        }

        public new async Task<APIResponseDTO> PatchAsync(Guid id, PatchUserRegistrationDTO dto)
        {
            using var transaction = await repository.BeginTransactionAsync();
            try
            {
                var dbEntity = await repository.GetByIdAsync(id);
                if (dbEntity == null)
                    return APIResponseDTO.Fail("Record not found.");

                if (dto.Photo != null)
                {
                    dto.PhotoPath = await imageHelper.UpdateImageAsync(dto.Photo, Constants.UserProfileImages, dbEntity.PhotoPath);
                    dto.Photo = null; // Clear the Photo property to avoid saving it again
                }

                var updatedFields = patchHelper.GetPatchedValues(dto);
                if (dto.Hobbies is { Count: > 0 })
                {
                    var newHobbies = dto.Hobbies.Distinct().Select(hobbyId => new TrnUserHobby
                    {
                        HobbyId = hobbyId,
                        UserId = dbEntity.Id
                    }).ToList();


                    await repository.UpdateHobbies(id, newHobbies);
                    updatedFields.Remove("Hobbies");
                }

                var result = await base.PatchAsync(id, updatedFields);
                await repository.CommitTransactionAsync();
                return result;
            }
            catch {
                await repository.RollbackTransactionAsync();
                throw;
            }
        }

        public new async Task<APIResponseDTO<GetUserResponseDTO>> GetByIdAsync(Guid id)
        {
            return await base.GetByIdAsync(id,query => query
                                                     .Include(u => u.Gender)
                                                     .Include(u => u.State)
                                                     .Include(u => u.City)
                                                     .Include(u => u.UserHobbies)
                                                        .ThenInclude(uh => uh.Hobby));
        }
    }

}

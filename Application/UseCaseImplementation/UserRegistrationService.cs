using Application.Helpers;
using Application.Helpers.Image;
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
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.UseCaseImplementation
{
    public class UserRegistrationService(
    IUserRegistrationRepository repository,
    IMapper mapper,
    IImageHelper imageHelper
) : GenericService<TrnUserRegistration, SaveUserResgistrationDTO, GetUserResponseDTO>(repository, mapper),
    IUserRegistrationService
    {
        public new async Task<APIResponseDTO> AddAsync(SaveUserResgistrationDTO dto)
        {
            var response = APIResponseDTO.Ok();

            // Save image if uploaded
            if (dto?.Photo != null)
            {
                dto.PhotoPath = await imageHelper.SaveImageAsync(dto.Photo, Constants.UserProfileImages);
            }

            if (dto.Id == Guid.Empty)
            {
                // ➕ Create new
                if (!await repository.CheckUserExistsByEmail(dto.Email))
                {
                    var user = _mapper.Map<TrnUserRegistration>(dto);
                    user.Id = Guid.NewGuid();
                    await _repository.AddAsync(user);
                    await _repository.SaveChangesAsync();
                    response = APIResponseDTO.Ok("User created successfully.");
                }
                else
                {
                    response = APIResponseDTO.Fail($"User already exists with email {dto.Email}!");
                }
            }
            else
            {
                var existingUser = await _repository.GetByIdAsync(dto.Id);

                if (existingUser == null)
                    return APIResponseDTO.Fail("User not found for update.");

                if (!string.IsNullOrWhiteSpace(dto.Name))
                    existingUser.Name = dto.Name;

                if (dto.Gender > 0)
                    existingUser.GenderId = dto.Gender;

                if (dto.DateOfBirth != default)
                    existingUser.DateOfBirth = dto.DateOfBirth;

                if (!string.IsNullOrWhiteSpace(dto.Email))
                    existingUser.Email = dto.Email;

                if (!string.IsNullOrWhiteSpace(dto.Mobile))
                    existingUser.Mobile = dto.Mobile;

                if (!string.IsNullOrWhiteSpace(dto.ContactNo))
                    existingUser.ContactNo = dto.ContactNo;

                if (dto.State > 0)
                    existingUser.StateId = dto.State;

                if (dto.City > 0)
                    existingUser.CityId = dto.City;

                if (!string.IsNullOrEmpty(dto.PhotoPath))
                    existingUser.PhotoPath = dto.PhotoPath;

                if (dto.Hobbies != null && dto.Hobbies.Count > 0)
                {
                    existingUser.UserHobbies?.Clear();
                    existingUser.UserHobbies = dto.Hobbies.Select(hobbyId => new TrnUserHobby
                    {
                        HobbyId = hobbyId,
                        UserId = existingUser.Id
                    }).ToList();
                }

                await _repository.UpdateAsync(existingUser);
                await _repository.SaveChangesAsync();

                return APIResponseDTO.Ok("User updated successfully.");
            }

            return response;
        }

        public async Task<APIResponseDTO<DataTableResponseDTO<GetUserResponseDTO>>> GetAllAsync(GetUserRequestListDTO dto)
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

        public async Task<APIResponseDTO<GetUserResponseDTO>> GetUserDetails(string email)
        {
            var response = await repository.GetUserDetails(email);
            return response.IsSuccess
                ? APIResponseDTO<GetUserResponseDTO>.Ok(mapper.Map<GetUserResponseDTO>(response.Data))
                : APIResponseDTO<GetUserResponseDTO>.Fail(response.Message);
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
    }

}

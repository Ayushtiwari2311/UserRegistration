using Application.UseCaseInterfaces;
using AutoMapper;
using DataTransferObjects.Request.Common;
using DataTransferObjects.Request.User;
using DataTransferObjects.Response.Common;
using DataTransferObjects.Response.User;
using Domain.Entities;
using Domain.RepositoryInterfaces;

namespace Application.UseCaseImplementation
{
    public class UserRegistrationService(IUserRegistrationRepository repository, IMapper mapper) : IUserRegistrationService
    { 
        public async Task<APIResponseDTO> AddAsync(SaveUserResgistrationDTO dto, FileUploadRequestDto photoDto)
        {
            var response = APIResponseDTO.Ok();
            if (!await repository.CheckUserExistsByEmail(dto.Email))
            {  
                if (photoDto != null)
                {
                    var uploadsFolder = Path.Combine("wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var newId = Guid.NewGuid();

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photoDto.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    await File.WriteAllBytesAsync(filePath, photoDto.FileData);

                    dto.PhotoPath = $"/uploads/{fileName}";
                }
                var user =  mapper.Map<TrnUserRegistration>(dto);
                await repository.AddAsync(user);
                response = APIResponseDTO.Ok("User registered!");
            }
            else
            {
                response = APIResponseDTO.Fail($"User already exists with {dto.Email}!");
            }
            return response;
        }

        public async Task<APIResponseDTO<DataTableResponseDTO<GetUserResponseDTO>>> GetAllAsync(GetUserRequestListDTO dto)
        {
            var usersRepData = await repository.GetAllAsync(dto);
            var userSerData = new DataTableResponseDTO<GetUserResponseDTO>
            {
                draw = dto.Draw,
                recordsTotal = usersRepData.recordsTotal,
                recordsFiltered = usersRepData.recordsFiltered,
                data = mapper.Map<List<GetUserResponseDTO>>(usersRepData.data)
            };
            return APIResponseDTO<DataTableResponseDTO<GetUserResponseDTO>>.Ok(userSerData);
        }

        public async Task<APIResponseDTO<GetUserResponseDTO>> GetUserDetails(string email)
        {
            var response = await repository.GetUserDetails(email);
            if (response.IsSuccess)
            {
                return APIResponseDTO<GetUserResponseDTO>.Ok(mapper.Map<GetUserResponseDTO>(response.Data));
            }
            else
            {
               return APIResponseDTO<GetUserResponseDTO>.Fail(response.Message);
            }
        }
    }
}

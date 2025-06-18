using Application.UseCaseInterfaces;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.RepositoryInterfaces;

namespace Application.UseCaseImplementation
{
    public class UserRegistrationService(IUserRegistrationRepository repository, IMapper mapper) : IUserRegistrationService
    { 
        public async Task<ResponseDTO> AddAsync(SaveUserResgistrationDTO dto, FileUploadDto photoDto)
        {
            var response = ResponseDTO.Ok();
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
                response = ResponseDTO.Ok("User registered!");
            }
            else
            {
                response = ResponseDTO.Fail($"User already exists with {dto.Email}!");
            }
            return response;
        }

        public async Task<DTODataTablesResponse<GetUserRegistrationDTO>> GetAllAsync(GetUserRegistrationListDTO dto)
        {
            var usersRepData = await repository.GetAllAsync(dto);
            return new DTODataTablesResponse<GetUserRegistrationDTO>
            {
                draw = dto.Draw,
                recordsTotal = usersRepData.recordsTotal,
                recordsFiltered = usersRepData.recordsFiltered,
                data = mapper.Map<List<GetUserRegistrationDTO>>(usersRepData.data)
            };
        }

        public async Task<ResponseDTO<GetUserRegistrationDTO>> GetUserDetails(string email)
        {
            var response = await repository.GetUserDetails(email);
            if (response.IsSuccess)
            {
                return ResponseDTO<GetUserRegistrationDTO>.Ok(mapper.Map<GetUserRegistrationDTO>(response.Data));
            }
            else
            {
               return ResponseDTO<GetUserRegistrationDTO>.Fail(response.Message);
            }
        }
    }
}

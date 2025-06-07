using Application.UseCaseInterfaces;
using Domain.DTOs;
using Domain.Entities;
using Domain.RepositoryInterfaces;

namespace Application.UseCaseImplementation
{
    public class UserRegistrationService(IUserRegistrationRepository repository) : IUserRegistrationService
    { 
        public async Task<ResponseDTO> AddAsync(SaveUserResgistrationDTO dto, FileUploadDto photoDto)
        {
            var response = new ResponseDTO();
            try
            {
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

                    var user = new TrnUserRegistration()
                    {
                        Name = dto.Name,
                        GenderId = dto.Gender,
                        DateOfBirth = dto.DateOfBirth.Date,
                        Email = dto.Email,
                        Mobile = dto.Mobile,
                        ContactNo = dto.ContactNo,
                        StateId = dto.State,
                        CityId = dto.City,
                        PhotoPath = dto.PhotoPath,
                        UserHobbies = dto.Hobbies.Select(hobby => new TrnUserHobby
                        {
                            HobbyId = hobby
                        }).ToList()
                    };
                    await repository.AddAsync(user);
                    response.IsSuccess = true;
                    response.Message = $"User registered!";
                }
                else
                {
                    response.Message = $"User already exists with {dto.Email}!";
                }
            }
            catch(Exception e)
            {   
                response.IsSuccess = false;
                response.Message = "Oops! Error occured please try again!";
            }
            return response;
        }

        public async Task<DTODataTablesResponse<GetUserRegistrationDTO>> GetAllAsync(GetUserRegistrationListDTO dto)
        {
            return await repository.GetAllAsync(dto);
        }
    }
}

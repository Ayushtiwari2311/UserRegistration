using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Azure.Core;
using Domain.DTOs;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Infrastructure.RepositoryImplementation
{
    public class UserRegistrationRepository(AppDbContext context) : IUserRegistrationRepository
    {
        public async Task AddAsync(TrnUserRegistration userRegistration)
        {
            await context.TrnUserRegistrations.AddAsync(userRegistration);
            await context.SaveChangesAsync();
        }

        public async Task<bool> CheckUserExistsByEmail(string email) => context.TrnUserRegistrations.Any(u => u.Email == email);

        public async Task<DTODataTablesResponse<GetUserRegistrationDTO>> GetAllAsync(GetUserRegistrationListDTO dto)
        {
            var query = context.TrnUserRegistrations
                        .Include(u => u.State)
                        .Include(u => u.City)
                        .Include(u => u.UserHobbies)
                            .ThenInclude(uh => uh.Hobby)
            .AsQueryable();

            var totalCount = await query.CountAsync();


            if (!string.IsNullOrEmpty(dto.searchValue))
                query = query.Where(u => u.Name.Contains(dto.searchValue));

            if (dto.StateId.HasValue)
                query = query.Where(u => u.StateId == dto.StateId);

            if (dto.CityId.HasValue)
                query = query.Where(u => u.CityId == dto.CityId);

            var totalCountFiltered = await query.CountAsync();

            var users = await query
                        .OrderBy(u => u.Name)
                        .Skip(dto.Start * dto.Length)
                        .Take(dto.Length)
                        .Select(u => new GetUserRegistrationDTO() {
                            Name = u.Name,
                            Gender = u.Gender.Name,
                            DateOfBirth = u.DateOfBirth,
                            Email = u.Email,
                            Mobile = u.Mobile,
                            ContactNo = u.ContactNo,
                            State = u.State.Name,
                            City = u.City.Name,
                            Hobbies = u.UserHobbies.Select(h => h.Hobby.Name).ToList(),
                            PhotoPath = u.PhotoPath
                        })
                        .ToListAsync();

            return new DTODataTablesResponse<GetUserRegistrationDTO>()
            {
                draw = dto.Draw,
                recordsTotal = totalCount,
                recordsFiltered = totalCountFiltered,
                data = users
            };
        }

    }
}

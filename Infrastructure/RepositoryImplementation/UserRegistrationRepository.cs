using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using static Azure.Core.HttpHeader;

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

        public async Task<DTODataTablesResponse<TrnUserRegistration>> GetAllAsync(GetUserRegistrationListDTO dto)
        {
            var query = context.TrnUserRegistrations
                        .Include(u => u.Gender)
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
                        .ToListAsync();

            return new DTODataTablesResponse<TrnUserRegistration>()
            {
                draw = dto.Draw,
                recordsTotal = totalCount,
                recordsFiltered = totalCountFiltered,
                data = users
            };
        }

        public async Task<ResponseDTO<TrnUserRegistration>> GetUserDetails(string email)
        {
            var user = context.TrnUserRegistrations
                        .Include(u => u.Gender)
                        .Include(u => u.State)
                        .Include(u => u.City)
                        .Include(u => u.UserHobbies)
                            .ThenInclude(uh => uh.Hobby)
                       .FirstOrDefault(u => u.Email == email);
            if (user is not null)
            {
                return ResponseDTO<TrnUserRegistration>.Ok(user);
            }
            else
            {
                return ResponseDTO<TrnUserRegistration>.Fail("No Record Found!");
            }
        }
    }
}

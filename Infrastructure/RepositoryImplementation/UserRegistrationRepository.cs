using DataTransferObjects.Request.User;
using DataTransferObjects.Response.Common;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.DatabaseContext;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoryImplementation
{
    internal class UserRegistrationRepository(AppDbContext context) : IUserRegistrationRepository
    {
        public async Task AddAsync(TrnUserRegistration userRegistration)
        {
            await context.TrnUserRegistrations.AddAsync(userRegistration);
            await context.SaveChangesAsync();
        }

        public async Task<bool> CheckUserExistsByEmail(string email) => context.TrnUserRegistrations.Any(u => u.Email == email);

        public async Task<DataTableResponseDTO<TrnUserRegistration>> GetAllAsync(GetUserRequestListDTO dto)
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

            var allowedSortColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "name", "Name" },
                    { "gender", "Gender.Name" },
                    { "state", "State.Name" },
                    { "city", "City.Name" },
                    { "dateOfBirth", "DateOfBirth" },
                    { "email", "Email" },
                    { "mobile", "Mobile" },
                    { "contactNo", "ContactNo" },
                    { "createdOn", "CreatedOn" }
                };

            string sortColumn = allowedSortColumns.TryGetValue(dto.SortColumn ?? string.Empty, out var mappedColumn)
                    ? mappedColumn
                    : "CreatedOn"; 

            bool ascending = dto.SortDirection?.ToLower() == "asc";

            // Apply sorting using dynamic extension
            query = query.OrderByDynamic(sortColumn, ascending);


            var users = await query
                        .Skip(dto.Start * dto.Length)
                        .Take(dto.Length)
                        .ToListAsync();

            return new DataTableResponseDTO<TrnUserRegistration>()
            {
                draw = dto.Draw,
                recordsTotal = totalCount,
                recordsFiltered = totalCountFiltered,
                data = users
            };
        }

        public async Task<APIResponseDTO<TrnUserRegistration>> GetUserDetails(string email)
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
                return APIResponseDTO<TrnUserRegistration>.Ok(user);
            }
            else
            {
                return APIResponseDTO<TrnUserRegistration>.Fail("No Record Found!");
            }
        }
    }
}

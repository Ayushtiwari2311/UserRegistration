using DataTransferObjects.Request.User;
using DataTransferObjects.Response.Common;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.DatabaseContext;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoryImplementation
{
    public class UserRegistrationRepository : GenericRepository<TrnUserRegistration>, IUserRegistrationRepository
    {
        public UserRegistrationRepository(AppDbContext context) : base(context) { }

        public async Task<bool> CheckUserExistsByEmail(string email)
            => await _context.TrnUserRegistrations.AnyAsync(u => u.Email == email);

        public async Task<DataTableResponseDTO<TrnUserRegistration>> GetAllAsync(GetUserRequestListDTO dto)
        {
            var query = _context.TrnUserRegistrations
                .Include(u => u.Gender)
                .Include(u => u.State)
                .Include(u => u.City)
                .Include(u => u.UserHobbies)
                    .ThenInclude(uh => uh.Hobby)
                .AsQueryable();

            int totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(dto.searchValue))
                query = query.Where(u => u.Name.Contains(dto.searchValue));

            if (dto.StateId.HasValue)
                query = query.Where(u => u.StateId == dto.StateId);

            if (dto.CityId.HasValue)
                query = query.Where(u => u.CityId == dto.CityId);

            int filteredCount = await query.CountAsync();

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

            string sortColumn = allowedSortColumns.TryGetValue(dto.SortColumn ?? "", out var mapped) ? mapped : "CreatedOn";
            bool ascending = dto.SortDirection?.ToLower() == "asc";
            query = query.OrderByDynamic(sortColumn, ascending);

            var users = await query
                .Skip(dto.Start * dto.Length)
                .Take(dto.Length)
                .ToListAsync();

            return new DataTableResponseDTO<TrnUserRegistration>
            {
                draw = dto.Draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = users
            };
        }

        public async Task<APIResponseDTO<TrnUserRegistration>> GetUserDetails(string email)
        {
            var user = await _context.TrnUserRegistrations
                .Include(u => u.Gender)
                .Include(u => u.State)
                .Include(u => u.City)
                .Include(u => u.UserHobbies)
                    .ThenInclude(uh => uh.Hobby)
                .FirstOrDefaultAsync(u => u.Email == email);

            return user != null
                ? APIResponseDTO<TrnUserRegistration>.Ok(user)
                : APIResponseDTO<TrnUserRegistration>.Fail("No Record Found!");
        }
    }
}

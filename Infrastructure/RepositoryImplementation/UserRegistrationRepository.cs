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

        public async Task UpdateHobbies(Guid userId, ICollection<TrnUserHobby> userHobbies)
        {
            var existingHobbies = await _context.MUserHobbies
                .Where(x => x.UserId == userId)
                .ToListAsync();

            _context.MUserHobbies.RemoveRange(existingHobbies);

            if (userHobbies is { Count: > 0 })
            {
                await _context.MUserHobbies.AddRangeAsync(userHobbies);
            }

            await _context.SaveChangesAsync();
        }
    }
}


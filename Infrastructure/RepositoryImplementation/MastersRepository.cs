using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DatabaseContext;
using Domain.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoryImplementation
{
    internal class MastersRepository(AppDbContext context) : IMastersRepository
    {
        public async Task<IEnumerable<MGender>> GetGendersAsync() => await context.MGenders.AsNoTracking().ToListAsync();
        public async Task<IEnumerable<MHobby>> GetHobbiesAsync() => await context.MHobbies.AsNoTracking().ToListAsync();
        public async Task<IEnumerable<MState>> GetStatesAsync() => await context.MStates.AsNoTracking().ToListAsync();
        public async Task<IEnumerable<MCity>> GetCitiesAsync(int stateId) => await context.MCities.AsNoTracking().Where(c => c.StateId == stateId).ToListAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.RepositoryInterfaces
{
    public interface IMastersRepository 
    {
        Task<IEnumerable<MGender>> GetGendersAsync();
        Task<IEnumerable<MHobby>> GetHobbiesAsync();
        Task<IEnumerable<MState>> GetStatesAsync();
        Task<IEnumerable<MCity>> GetCitiesAsync(int stateId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs;

namespace Application.UseCaseInterfaces
{
    public interface IMastersService
    {
        Task<IEnumerable<DTODropDown>> GetGendersAsync();
        Task<IEnumerable<DTODropDown>> GetHobbiesAsync();
        Task<IEnumerable<DTODropDown>> GetStatesAsync();
        Task<IEnumerable<DTODropDown>> GetCitiesAsync(int stateId);
    }
}

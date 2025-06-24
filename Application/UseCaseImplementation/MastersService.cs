using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCaseInterfaces;
using DataTransferObjects.Response.Common;
using Domain.RepositoryInterfaces;

namespace Application.UseCaseImplementation
{
    public class MastersService(IMastersRepository repository) : IMastersService
    {
        public async Task<IEnumerable<DropDownResponseDTO>> GetCitiesAsync(int stateId)
        {
            var cities = await repository.GetCitiesAsync(stateId);
            return cities.Select(city => new DropDownResponseDTO
            {
                Id = city.Id,
                Name = city.Name
            }).ToList();
        }

        public async Task<IEnumerable<DropDownResponseDTO>> GetGendersAsync()
        {
            var genders = await repository.GetGendersAsync();
            return genders.Select(gender => new DropDownResponseDTO
            {
                Id = gender.Id,
                Name = gender.Name
            }).ToList();
        }

        public async Task<IEnumerable<DropDownResponseDTO>> GetHobbiesAsync()
        {
            var hobbies = await repository.GetHobbiesAsync();
            return hobbies.Select(hobby => new DropDownResponseDTO
            {
                Id = hobby.Id,
                Name = hobby.Name
            }).ToList();
        }

        public async Task<IEnumerable<DropDownResponseDTO>> GetStatesAsync()
        {
            var states = await repository.GetStatesAsync();
            return states.Select(state => new DropDownResponseDTO
            {
                Id = state.Id,
                Name = state.Name
            }).ToList();
        }
    }
}

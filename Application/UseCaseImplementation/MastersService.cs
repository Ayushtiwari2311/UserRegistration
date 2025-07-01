using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCaseInterfaces;
using DataTransferObjects.Response.Common;
using Domain.Entities;
using Domain.RepositoryInterfaces;

namespace Application.UseCaseImplementation
{
    public class MastersService(IMastersRepository repository) : IMastersService
    {
        public async Task<APIResponseDTO<IEnumerable<DropDownResponseDTO>>> GetCitiesAsync(int stateId)
        {
            var cities = await repository.GetCitiesAsync(stateId);
            return APIResponseDTO<IEnumerable< DropDownResponseDTO >>.Ok(cities.Select(city => new DropDownResponseDTO
            {
                Id = city.Id,
                Name = city.Name
            }).ToList());
        }

        public async Task<APIResponseDTO<IEnumerable<DropDownResponseDTO>>> GetGendersAsync()
        {
            var genders = await repository.GetGendersAsync();
            return APIResponseDTO<IEnumerable<DropDownResponseDTO>>.Ok(genders.Select(gender => new DropDownResponseDTO
            {
                Id = gender.Id,
                Name = gender.Name
            }).ToList());
        }

        public async Task<APIResponseDTO<IEnumerable<DropDownResponseDTO>>> GetHobbiesAsync()
        {
            var hobbies = await repository.GetHobbiesAsync();
            return APIResponseDTO<IEnumerable<DropDownResponseDTO>>.Ok(hobbies.Select(hobby => new DropDownResponseDTO
            {
                Id = hobby.Id,
                Name = hobby.Name
            }).ToList());
        }

        public async Task<APIResponseDTO<IEnumerable<DropDownResponseDTO>>> GetStatesAsync()
        {
            var states = await repository.GetStatesAsync();
            return APIResponseDTO<IEnumerable<DropDownResponseDTO>>.Ok(states.Select(state => new DropDownResponseDTO
            {
                Id = state.Id,
                Name = state.Name
            }).ToList());
        }
    }
}

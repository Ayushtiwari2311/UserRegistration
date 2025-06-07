using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCaseInterfaces;
using Domain.DTOs;
using Domain.RepositoryInterfaces;

namespace Application.UseCaseImplementation
{
    public class MastersService(IMastersRepository repository) : IMastersService
    {
        public async Task<IEnumerable<DTODropDown>> GetCitiesAsync(int stateId)
        {
            var cities = await repository.GetCitiesAsync(stateId);
            return cities.Select(city => new DTODropDown
            {
                Id = city.Id,
                Name = city.Name
            }).ToList();
        }

        public async Task<IEnumerable<DTODropDown>> GetGendersAsync()
        {
            var genders = await repository.GetGendersAsync();
            return genders.Select(gender => new DTODropDown
            {
                Id = gender.Id,
                Name = gender.Name
            }).ToList();
        }

        public async Task<IEnumerable<DTODropDown>> GetHobbiesAsync()
        {
            var hobbies = await repository.GetHobbiesAsync();
            return hobbies.Select(hobby => new DTODropDown
            {
                Id = hobby.Id,
                Name = hobby.Name
            }).ToList();
        }

        public async Task<IEnumerable<DTODropDown>> GetStatesAsync()
        {
            var states = await repository.GetStatesAsync();
            return states.Select(state => new DTODropDown
            {
                Id = state.Id,
                Name = state.Name
            }).ToList();
        }
    }
}

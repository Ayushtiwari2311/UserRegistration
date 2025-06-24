using AutoMapper;
using DataTransferObjects.Request.User;
using DataTransferObjects.Response.User;
using Domain.Entities;

namespace Application.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<TrnUserRegistration, GetUserResponseDTO>()
            .ForMember(user => user.Gender, opt => opt.MapFrom(src => src.Gender.Name))
            .ForMember(user => user.State, opt => opt.MapFrom(src => src.State.Name))
            .ForMember(user => user.City, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(user => user.Hobbies, opt => opt.MapFrom(src => src.UserHobbies.Select(h => h.Hobby.Name).ToList()));

            CreateMap<SaveUserResgistrationDTO, TrnUserRegistration>()
                .ForMember(dest => dest.GenderId, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Gender, opt => opt.Ignore())
                .ForMember(dest => dest.State, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.UserHobbies, opt => opt.MapFrom(src =>
                    src.Hobbies.Select(hobbyId => new TrnUserHobby
                    {
                        HobbyId = hobbyId
                    }).ToList()));
        }
        
    }
}

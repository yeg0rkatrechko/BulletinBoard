using AutoMapper;
using Models;

namespace Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Advert, AdvertDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Reactions, opt => opt.MapFrom(src => src.AdvertReaction));
            CreateMap<User, UserDto>();
        }
    }
}

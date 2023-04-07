using AutoMapper;
using Models;

namespace Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Advert, AdvertDto>();
            CreateMap<User, UserDto>();
        }
    }
}

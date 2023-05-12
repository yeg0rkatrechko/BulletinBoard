using AutoMapper;
using Domain;
using Services.Models;

namespace Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Advert, AdvertDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.ReactionSum, opt => opt.MapFrom(src => src.AdvertReaction != null ? src.AdvertReaction.Sum(r => (int)r.Reaction) : 0))
                .ForMember(dest => dest.AdvertImages, opt => opt.MapFrom(src => src.AdvertImages != null ? src.AdvertImages.Select(image => image.FilePath).ToList() : null));
            CreateMap<User, UserDto>();
            CreateMap<AdvertImage, AdvertImageDto>();
        }
    }
}

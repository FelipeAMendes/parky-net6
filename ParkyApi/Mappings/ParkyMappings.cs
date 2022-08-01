using AutoMapper;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;

namespace ParkyApi.Mappings;

public class ParkyMappings : Profile
{
    public ParkyMappings()
    {
        CreateMap<NationalPark, NationalParkDto>().ReverseMap();

        CreateMap<Trail, TrailDto>().ReverseMap();
        CreateMap<Trail, TrailCreateDto>().ReverseMap();
        CreateMap<Trail, TrailUpdateDto>().ReverseMap();
        CreateMap<Trail, TrailV2Dto>()
            .ForMember(dest => dest.TrailName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.TrailDistance, opt => opt.MapFrom(src => src.Distance))
            .ForMember(dest => dest.TrailDifficult, opt => opt.MapFrom(src => src.Difficult))
            .ReverseMap();

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => string.Empty))
            .ReverseMap();
    }
}

using AspNetApp.Dto;
using AspNetApp.Entity;
using AutoMapper;

namespace AspNetApp.Mapping;

/// <summary>
/// Declares configuration of (BusinessLogic - Endpoints) layers mapping
/// </summary>
public class AppToDbEndpointsProfile : Profile
{
    /// <summary>
    /// Это конструктор, нахрена ему комментарии?
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public AppToDbEndpointsProfile()
    {
        CreateMap<MoveSpaceshipRequest, Star>()
            .ForMember(s => s.VisitedByShips, opt => opt.Ignore())
            .ForMember(s => s.AngleMilliradians, opt => opt.MapFrom(src => src.NewAngle))
            .ForMember(s => s.Radius, opt => opt.MapFrom(src => src.NewRadius));
        
        CreateMap<StarVisitRequest, Star>()
            .ForMember(s => s.VisitedByShips, opt => opt.Ignore());

        CreateMap<Player, UserDto>();
    }
}
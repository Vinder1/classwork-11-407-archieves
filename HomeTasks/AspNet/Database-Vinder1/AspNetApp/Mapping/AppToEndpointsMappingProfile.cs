using AspNetApp.Entity;
using AutoMapper;

namespace AspNetApp.Mapping;

public class AppToDbEndpointsProfile : Profile
{
    public AppToDbEndpointsProfile()
    {
        CreateMap<Dto.MoveSpaceshipRequest, Star>()
            .ForMember(s => s.VisitedByShips, opt => opt.Ignore())
            .ForMember(s => s.AngleMilliradians, opt => opt.MapFrom(src => src.NewAngle))
            .ForMember(s => s.Radius, opt => opt.MapFrom(src => src.NewRadius));
        
        CreateMap<Dto.StarVisitRequest, Star>()
            .ForMember(s => s.VisitedByShips, opt => opt.Ignore());
    }
}
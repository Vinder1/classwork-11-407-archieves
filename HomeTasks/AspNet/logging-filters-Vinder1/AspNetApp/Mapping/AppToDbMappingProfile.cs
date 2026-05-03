using AspNetApp.Entity;
using AutoMapper;

namespace AspNetApp.Mapping;

/// <summary>
/// Declares configuration of (BusinessLogic - Db) layers mapping
/// </summary>
public class AppToDbMappingProfile : Profile
{
    /// <summary>
    /// Это конструктор, нахрена ему комментарии?
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public AppToDbMappingProfile()
    {
        CreateMap<Star, Repository.DbEntity.Star>()
            .ForMember(dest => dest.GalaxyVersion, opt => opt.MapFrom((src, dest, destMember, context) =>
            {
                return context.Items.TryGetValue("GalaxyVersion", out var version)
                    ? (int)version
                    : throw new InvalidOperationException("GalaxyVersion not provided in mapping context");
            }))
            .ForMember(dest => dest.Galaxy, opt => opt.Ignore());

        CreateMap<Repository.DbEntity.Star, Star>();
        
        CreateMap<Spaceship, Repository.DbEntity.Spaceship>()
            .ForMember(dest => dest.Galaxy, opt => opt.Ignore())
            .ForMember(dest => dest.Owner, opt => opt.Ignore());
        CreateMap<Repository.DbEntity.Spaceship, Spaceship>();

        CreateMap<Player, Repository.DbEntity.Player>().PreserveReferences().ReverseMap();

        CreateMap<Repository.DbEntity.Galaxy, Galaxy>()
            .ConvertUsing<GalaxyConverter>();
        CreateMap<Galaxy, Repository.DbEntity.Galaxy>()
            .ConvertUsing<GalaxyReverseConverter>();

        CreateMap<SpaceshipStarVisit, Repository.DbEntity.SpaceshipStarVisit>()
            .ForMember(dest => dest.StarGalaxyVersion, opt => opt.MapFrom(src => src.GalaxyVersion))
            .ForMember(dest => dest.StarRadius, opt => opt.MapFrom(src => src.Star.Radius))
            .ForMember(dest => dest.StarAngleMilliradians, opt => opt.MapFrom(src => src.Star.AngleMilliradians))
            .ForMember(dest => dest.Star, opt => opt.Ignore())
            .ForMember(dest => dest.Spaceship, opt => opt.Ignore())
            .ForMember(dest => dest.SpaceshipId, opt => opt.MapFrom(src => src.Spaceship.OwnerId));

        CreateMap<Repository.DbEntity.SpaceshipStarVisit, SpaceshipStarVisit>()
            .ForMember(dest => dest.Star, opt => opt.MapFrom(src => src.Star))
            .ForMember(dest => dest.Spaceship, opt => opt.MapFrom(src => src.Spaceship))
            .ForMember(dest => dest.GalaxyVersion, opt => opt.MapFrom(src => src.StarGalaxyVersion));
    }
}
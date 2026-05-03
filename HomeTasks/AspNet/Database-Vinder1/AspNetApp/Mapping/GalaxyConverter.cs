namespace AspNetApp.Mapping;

using AspNetApp.Entity;
using AutoMapper;

public class GalaxyConverter(IMapper mapper) : ITypeConverter<Repository.DbEntity.Galaxy, Galaxy>
{
    private readonly IMapper _mapper = mapper;

    public Galaxy Convert(
        Repository.DbEntity.Galaxy source,
        Galaxy destination,
        ResolutionContext context)
    {
        if (source == null || source.Stars.Count == 0)
            return new Galaxy
            {
                Version = source?.Version ?? 0,
                Stars = new StarCollection([])
            };

        // 1. Маппинг Entity.Star → Domain.Star
        var domainStars = _mapper.Map<List<Star>>(source.Stars);

        // 2. Группировка в 2D массив (например, по Radius)
        var stars2D = GroupStarsByRadius(domainStars);

        // 3. Создание StarCollection
        return new Galaxy
        {
            Version = source.Version,
            Stars = new StarCollection(stars2D)
        };
    }

    private static Star[][] GroupStarsByRadius(List<Star> stars)
    {
        return [ [], .. stars
            .GroupBy(s => s.Radius)
            .OrderBy(g => g.Key)
            .Select(g => g.OrderBy(s => s.AngleMilliradians).ToArray())];
    }
}
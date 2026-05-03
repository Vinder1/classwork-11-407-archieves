namespace AspNetApp.Mapping;

using AspNetApp.Entity;
using AutoMapper;
/// <summary>
/// Конвертирует IStarCollection (domain) → ICollection&lt;Star&gt; (EF entity)
/// Требует передачи GalaxyVersion через ResolutionContext.Items["GalaxyVersion"]
/// </summary>
public class GalaxyReverseConverter(IMapper mapper) : ITypeConverter<Galaxy, Repository.DbEntity.Galaxy>
{
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Repository.DbEntity.Galaxy Convert(
        Galaxy source, 
        Repository.DbEntity.Galaxy destination, 
        ResolutionContext context)
    {
        if (source?.Stars == null)
            return new Repository.DbEntity.Galaxy
            {
                Version = source?.Version ?? 0,
                Stars = []
            };

        // 1. Flatten 2D массив в плоский список
        var domainStars = source.Stars.GetAllStars().ToList();

        // 2. Маппинг Domain.Star → Entity.Star
        var entityStars = _mapper.Map<ICollection<Repository.DbEntity.Star>>(domainStars, opts =>
        {
            opts.Items["GalaxyVersion"] = source.Version;
        });

        return new Repository.DbEntity.Galaxy
        {
            Version = source.Version,
            Stars = entityStars
        };
    }
}
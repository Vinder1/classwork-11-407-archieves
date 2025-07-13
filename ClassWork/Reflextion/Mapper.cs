using System.Reflection;

namespace Reflextion;

public static class Mapper
{
    public static TDestination Map<TSource, TDestination>(TSource source) where TDestination : new()
    {
        var destination = new TDestination();
        
        var sourceType = typeof(TSource);
        var destinationType = typeof(TDestination);
        var sourceProps = sourceType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var destinationProps = destinationType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public);

        foreach (var prop in sourceProps)
        {
            var destProp = destinationProps
                .FirstOrDefault(p => p.Name == prop.Name && p.PropertyType == prop.PropertyType);
            if (destProp != null)
            {
                destProp.SetValue(destination, prop.GetValue(source));
            }
        }
        
        return destination;
    }
}
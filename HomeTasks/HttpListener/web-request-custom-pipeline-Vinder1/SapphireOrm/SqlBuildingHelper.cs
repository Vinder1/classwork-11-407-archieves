using System.Reflection;
using SapphireOrm.Entities;

namespace SapphireOrm;

public static class SqlBuildingHelper
{
    public static string GetTable<TEntity>()
    {
        var entityAttribute = typeof(TEntity).GetCustomAttribute<EntityAttribute>()
            ?? throw new Exception("Attribute missing");
        return entityAttribute.Table;
    }
}

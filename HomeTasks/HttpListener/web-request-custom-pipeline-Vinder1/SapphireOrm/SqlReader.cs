using System.Data.Common;
using System.Reflection;
using SapphireOrm.Entities;

namespace SapphireOrm;

public static class SqlReader
{
    public async static Task<IReadOnlyList<TEntity>> ReadMultiple<TEntity>(DbDataReader reader) where TEntity : new()
    {
        var list = new List<TEntity>();
        while (await reader.ReadAsync())
        {
            list.Add(GetSingleNotNullProvided<TEntity>(reader));
        }
        return list.AsReadOnly();
    }

    public async static Task<TEntity?> ReadSingle<TEntity>(DbDataReader reader) where TEntity : new()
    {
        if (await reader.ReadAsync())
        {
            return GetSingleNotNullProvided<TEntity>(reader);
        }
        return default;
    }

    private static T GetSingleNotNullProvided<T>(DbDataReader reader) where T : new()
    {
        var newObject = new T();
        var properties = typeof(T).GetProperties();

        foreach (var prop in properties)
        {
            var binding = prop.GetCustomAttribute<BindAttribute>();
            if (binding == null)
                continue;

            var columnName = binding.Column;

            int ordinal;
            try
            {
                ordinal = reader.GetOrdinal(columnName);
            } catch
            {
                continue;
            }

            if (reader.IsDBNull(ordinal))
            {
                if (prop.PropertyType.IsValueType && Nullable.GetUnderlyingType(prop.PropertyType) == null)
                {
                    continue;
                }
                prop.SetValue(newObject, null);
                continue;
            }

            var value = reader.GetValue(ordinal);
            var targetType = prop.PropertyType;

            if (value.GetType() != targetType)
            {
                value = Convert.ChangeType(value, Nullable.GetUnderlyingType(targetType) ?? targetType);
            }

            prop.SetValue(newObject, value);
        }

        return newObject;
    }
}

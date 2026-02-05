using System.Reflection;
using SapphireOrm.Entities;

namespace SapphireOrm.Builders;

public class SqlInsertBuilder : SqlBuilder
{    
    private record Binding(string Column, object? Value, Type Type);

    public static SqlInsertBuilder Create<TEntity>()
    {
        var builder = new SqlInsertBuilder();
        var bindings = GetBindings<TEntity>().ToArray();
        var table = SqlBuildingHelper.GetTable<TEntity>();

        builder.command.Append($"insert into {table} ({string.Join(", ", bindings)}) ");
        builder.command.Append($"values ({string.Join(", ", bindings.Select(b => '@' + b))})");
        return builder;
    }

    public SqlInsertBuilder Returning(string column)
    {
        command.Append(" returning ");
        command.Append(column);
        return this;
    }

    private static IEnumerable<string> GetBindings<TEntity>()
    {
        var bindings = typeof(TEntity).GetProperties()
            .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(BindAttribute)))
            .Select(p => p.GetCustomAttributes(typeof(BindAttribute)).OfType<BindAttribute>().First().Column);

        bindings.Concat(typeof(TEntity).GetFields()
            .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(BindAttribute)))
            .Select(p => p.GetCustomAttributes(typeof(BindAttribute)).OfType<BindAttribute>().First().Column));

        return bindings;
    }

    private static IEnumerable<Binding> GetBindings<TEntity>(TEntity entity)
    {
        var bindings = entity!.GetType().GetProperties()
            .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(BindAttribute)))
            .Select(p => new Binding
            (
                Column: p.GetCustomAttributes(typeof(BindAttribute)).OfType<BindAttribute>().First().Column,
                Value: p.GetValue(entity),
                Type: p.GetType()
            ));

        bindings.Concat(entity.GetType().GetFields()
            .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(BindAttribute)))
            .Select(p => new Binding
            (
                Column: p.GetCustomAttributes(typeof(BindAttribute)).OfType<BindAttribute>().First().Column,
                Value: p.GetValue(entity),
                Type: p.GetType()
            )));

        return bindings;
    }
}

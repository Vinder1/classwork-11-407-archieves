using System.Reflection;
using SapphireOrm.Entities;

namespace SapphireOrm.Builders;

public class SqlSelectBuilder : SqlBuilder
{
    public static SqlSelectBuilder Create<TEntity>()
    {
        var builder = new SqlSelectBuilder();

        var tableName = SqlBuildingHelper.GetTable<TEntity>();

        builder.command.Append($"select * from {tableName}");
        return builder;
    }

    public static SqlSelectBuilder Create<TEntity>(params string[] fields)
    {
        var builder = new SqlSelectBuilder();

        var tableName = SqlBuildingHelper.GetTable<TEntity>();

        builder.command.Append($"select {string.Join(", ", fields)} from {tableName}");
        return builder;
    }

    public SqlSelectBuilder Where(string condition)
    {
        command.Append(" where ");
        command.Append(condition);
        return this;
    }

    /// <summary>
    /// ' where {column} =  @{column}'
    /// </summary>
    public SqlSelectBuilder By(string column)
    {
        return Where($"{column} = @{column}");
    }
}

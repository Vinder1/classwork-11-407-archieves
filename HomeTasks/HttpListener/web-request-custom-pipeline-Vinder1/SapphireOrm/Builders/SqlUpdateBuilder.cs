using System.Runtime.CompilerServices;

namespace SapphireOrm.Builders;

public class SqlUpdateBuilder : SqlBuilder
{
    public static SqlUpdateBuilder Create<TEntity>(params string[] columns)
    {
        var builder = new SqlUpdateBuilder();

        var tableName = SqlBuildingHelper.GetTable<TEntity>();

        var updateColumns = string.Join(", ", columns.Select(x => $"{x} = @{x}"));

        builder.command.Append($"update {tableName} set {updateColumns}");
        return builder;
    }

    public SqlUpdateBuilder Where(string condition)
    {
        command.Append(" where ");
        command.Append(condition);
        return this;
    }

    /// <summary>
    /// ' where {column} =  @{column}'
    /// </summary>
    public SqlUpdateBuilder By(string column)
    {
        return Where($"{column} = @{column}");
    }

    public SqlUpdateBuilder Returning(string column)
    {
        command.Append(" returning ");
        command.Append(column);
        return this;
    }
}

namespace SapphireOrm.Builders;

public class SqlDeleteBuilder : SqlBuilder
{
    public static SqlDeleteBuilder Create<TEntity>()
    {
        var builder = new SqlDeleteBuilder();

        var tableName = SqlBuildingHelper.GetTable<TEntity>();

        builder.command.Append($"delete from {tableName}");
        return builder;
    }

    public SqlDeleteBuilder Where(string condition)
    {
        command.Append(" where ");
        command.Append(condition);
        return this;
    }

    /// <summary>
    /// ' where {column} =  @{column}'
    /// </summary>
    public SqlDeleteBuilder By(string column)
    {
        return Where($"{column} = @{column}");
    }

    public SqlDeleteBuilder Returning(string column)
    {
        command.Append(" returning ");
        command.Append(column);
        return this;
    }
}

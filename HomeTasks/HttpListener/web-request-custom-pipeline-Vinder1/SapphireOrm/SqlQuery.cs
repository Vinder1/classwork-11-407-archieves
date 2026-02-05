using SapphireOrm.Builders;

namespace SapphireOrm
{
    public class SqlQuery
    {
        public static SqlSelectBuilder SelectFrom<T>() => SqlSelectBuilder.Create<T>();
        public static SqlSelectBuilder SelectFrom<T>(params string[] fields) => SqlSelectBuilder.Create<T>(fields);
        public static SqlInsertBuilder InsertValues<T>() => SqlInsertBuilder.Create<T>();
        public static SqlDeleteBuilder DeleteFrom<T>() => SqlDeleteBuilder.Create<T>();
        public static SqlUpdateBuilder UpdateSet<T>(params string[] args) => SqlUpdateBuilder.Create<T>(args);
    }
}
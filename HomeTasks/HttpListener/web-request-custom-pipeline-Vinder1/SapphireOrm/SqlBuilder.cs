using System.Text;

namespace SapphireOrm;

public abstract class SqlBuilder
{
    public string ToSql() => command.Append(';').ToString();
    protected readonly StringBuilder command = new();
}

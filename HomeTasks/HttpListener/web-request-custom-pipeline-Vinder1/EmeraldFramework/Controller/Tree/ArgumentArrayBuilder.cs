namespace Emerald.Controller.Tree;

public class ArgumentArrayBuilder(string path, AppContext context)
{
    public string Current
        => pathParts[currentIndex];

    public bool End
        => currentIndex >= pathParts.Length;

    private readonly string[] pathParts = path.Split('/').Where(s => s != "").ToArray();
    private readonly List<object> arguments = [context];
    private int currentIndex = 0;

    public void MoveNext(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Const:
                break;
            case ResourceType.IntPattern:
                arguments.Add(int.Parse(Current));
                break;
            case ResourceType.StringPattern:
                arguments.Add(Current);
                break;
            default:
                break;
        }
        currentIndex++;
    }

    public object[] Build()
        => arguments.ToArray();
}

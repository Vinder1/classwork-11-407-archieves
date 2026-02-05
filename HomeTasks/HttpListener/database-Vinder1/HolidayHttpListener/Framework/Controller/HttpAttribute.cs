namespace HolidayHttpListener.Framework.Controller;

[AttributeUsage(AttributeTargets.Method)]
public class HttpAttribute(string Path, HttpMethod Method) : Attribute
{
    public string Path { get; } = Path;
    public HttpMethod Method { get; } = Method;
}
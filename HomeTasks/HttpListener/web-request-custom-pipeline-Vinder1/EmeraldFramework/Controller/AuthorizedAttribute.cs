namespace Emerald.Controller;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class AuthorizedAttribute(string role = "any", string? redirect = null) : Attribute
{
    public string Role { get; } = role;
    public string? Redirect { get; } = redirect;
}

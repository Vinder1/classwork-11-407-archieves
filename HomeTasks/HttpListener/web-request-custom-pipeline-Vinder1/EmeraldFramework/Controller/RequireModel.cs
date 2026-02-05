namespace EmeraldFramework.Controller;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class RequireModelAttribute<TModelType> : Attribute {}

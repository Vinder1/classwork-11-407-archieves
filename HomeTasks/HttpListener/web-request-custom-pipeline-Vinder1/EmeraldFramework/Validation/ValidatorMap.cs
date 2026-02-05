using Emerald.Services;

namespace Emerald.Validation;

public class ValidatorMap
{
    private Dictionary<Type, object> _validators = [];

    public void Register<TModel, TValidator>(ServiceCollection services) where TValidator : IValidator<TModel>
    {
        _validators.Add(typeof(TModel), services.GetSingleton<TValidator>()!);
    }

    public object? Resolve(Type t) => _validators.TryGetValue(t, out var v) ? v : null; 
}
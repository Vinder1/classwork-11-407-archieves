using Emerald;
using Emerald.Exceptions;
using Emerald.Middleware;
using Emerald.Validation;
using FluentValidation;
using FluentValidation.Results;

namespace HolidayApp.Middleware;

public class ValidationMiddleware(ValidatorMap map) : IMiddleware
{
    public IMiddleware? Next { set; get; }

    public async Task InvokeAsync(Emerald.AppContext context)
    {
        var model = context.Model;
        if (model == null)
        {
            await MoveNext(context);
            return;
        }
        var modelType = model.GetType();

        var validator = map.Resolve(model.GetType());
        if (validator == null)
        {
            await MoveNext(context);
            return;
        }

        var closedType = typeof(AbstractValidator<>).MakeGenericType(modelType);
        var vType = validator.GetType();
        if (!vType.IsAssignableTo(closedType))
        {
            throw new PipelineBuildingException($"Invalid validator for type {modelType} : {vType}");
        }

        var method = vType.GetMethod("ValidateAsync", [modelType, typeof(CancellationToken)]);
        if (method == null)
        {
            throw new PipelineBuildingException($"Method 'ValidateAsync' in {vType} not found");
        }
        var result = await (Task<ValidationResult>)method.Invoke(validator, [model, context.CancellationToken])!;
        if (result == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        if (!result.IsValid)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            await context.WriteJson(new
            {
                errors = result.Errors.Select(e => e.ErrorMessage).ToList()
            });
            return;
        }
        
        await MoveNext(context);
    }

    private async Task MoveNext(Emerald.AppContext context)
    {
        if (Next == null)
            return;
        await Next.InvokeAsync(context);
    }
}

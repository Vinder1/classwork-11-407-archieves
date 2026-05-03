using AspNetApp.Validation;
using FluentValidation;

namespace AspNetApp.PipelineConfiguration;

/// <summary>
/// 
/// </summary>
public static class ValidationConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    public static void AddValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<SignInRequestValidator>();
    }
}
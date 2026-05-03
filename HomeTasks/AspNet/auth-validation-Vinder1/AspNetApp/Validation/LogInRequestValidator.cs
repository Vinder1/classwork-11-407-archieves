using AspNetApp.Dto;
using FluentValidation;

namespace AspNetApp.Validation;

/// <summary>
/// 
/// </summary>
public class LogInRequestValidator : AbstractValidator<LogInRequest>
{
    /// <summary>
    /// 
    /// </summary>
    public LogInRequestValidator()
    {
        RuleFor(x => x.Login)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Логин обязателен")
            .MinimumLength(3)
                .WithMessage("Логин должен содержать минимум 3 символа")
            .MaximumLength(30)
                .WithMessage("Логин не должен превышать 30 символов")
            .Matches(@"^[a-zA-Z0-9_]+$")
                .WithMessage("Логин может содержать только латинские буквы, цифры и подчёркивание")
            .WithName("Логин");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Пароль обязателен")
            .MinimumLength(8)
                .WithMessage("Пароль должен содержать минимум 8 символов")
            .MaximumLength(100)
                .WithMessage("Пароль не должен превышать 100 символов")
            .Matches(@"[A-Z]+")
                .WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
            .Matches(@"[a-z]+")
                .WithMessage("Пароль должен содержать хотя бы одну строчную букву")
            .Matches(@"[0-9]+")
                .WithMessage("Пароль должен содержать хотя бы одну цифру")
            .WithName("Пароль");
    }
}
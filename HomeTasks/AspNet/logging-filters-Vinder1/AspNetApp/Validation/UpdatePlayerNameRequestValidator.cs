using AspNetApp.Dto;
using FluentValidation;

namespace AspNetApp.Validation;

/// <summary>
/// 
/// </summary>
public class UpdatePlayerNameRequestValidator : AbstractValidator<UpdatePlayerNameRequest>
{
    /// <summary>
    /// 
    /// </summary>
    public UpdatePlayerNameRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("Имя пользователя обязательно")
            .MinimumLength(2)
                .WithMessage("Имя должно содержать минимум 2 символа")
            .MaximumLength(50)
                .WithMessage("Имя не должно превышать 50 символов")
            .Matches(@"^[a-zA-Zа-яА-ЯёЁ0-9\s\-_']+$")
                .WithMessage("Имя может содержать только буквы, цифры, пробелы, дефис, подчёркивание и апостроф")
            .WithName("Имя");
    }
}
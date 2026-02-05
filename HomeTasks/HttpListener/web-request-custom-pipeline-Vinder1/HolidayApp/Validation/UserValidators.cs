using FluentValidation;
using HolidayApp.Dto;

namespace HolidayApp.Validation;

public class UserCreateValidator : AbstractValidator<SignUpDto>, Emerald.Validation.IValidator<SignUpDto>
{
    public UserCreateValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty()
            .Matches("^[a-zA-Zа-яА-ЯёЁ0-9_]{4,25}$")
            .WithMessage("Имя должно быть от 4 до 25 символов и состоять только из '_' и латинских символов / символов кириллицы.");

        RuleFor(user => user.Login)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9_]{4,20}$")
            .WithMessage("Логин должен быть от 4 до 20 символов и состоять только из '_' и латинских символов.");

        RuleFor(user => user.Password)
            .Matches("^(?=.*[a-zA-Z])(?=.*\\d).{8,}$") // мерзость
            .WithMessage("Пароль должен состоять минимум из 8 символов и содержать и латинские буквы, и цифры."); 
    }
}

public class UserUpdateValidator : AbstractValidator<UpdateUserDto>, Emerald.Validation.IValidator<UpdateUserDto>
{
    public UserUpdateValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty()
            .Matches("^[a-zA-Zа-яА-ЯёЁ0-9_]{4,25}$")
            .WithMessage("Имя должно быть от 4 до 25 символов и состоять только из '_' и латинских символов / символов кириллицы.");

        RuleFor(user => user.Password)
            .Matches("^(?=.*[a-zA-Z])(?=.*\\d).{8,}$") // мерзость
            .WithMessage("Пароль должен состоять минимум из 8 символов и содержать и латинские буквы, и цифры.");
    }
}
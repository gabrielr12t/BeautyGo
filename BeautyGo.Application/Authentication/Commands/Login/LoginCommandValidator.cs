using BeautyGo.Application.Core.Errors;
using BeautyGo.Application.Core.Extensions;
using FluentValidation;

namespace BeautyGo.Application.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithError(ValidationErrors.Login.EmailIsRequired);

        RuleFor(x => x.Password).NotEmpty().WithError(ValidationErrors.Login.PasswordIsRequired);
    }
}

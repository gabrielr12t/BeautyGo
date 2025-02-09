using BeautyGo.Application.Core.Errors;
using BeautyGo.Application.Core.Extensions;
using FluentValidation;

namespace BeautyGo.Application.Users.Commands.CreateUser;

internal class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithError(ValidationErrors.CreateUser.FirstNameIsRequired);

        RuleFor(x => x.LastName).NotEmpty().WithError(ValidationErrors.CreateUser.LastNameIsRequired);

        RuleFor(x => x.Email).NotEmpty().WithError(ValidationErrors.CreateUser.EmailIsRequired);

        RuleFor(x => x.Password).NotEmpty().WithError(ValidationErrors.CreateUser.PasswordIsRequired);
    }
}

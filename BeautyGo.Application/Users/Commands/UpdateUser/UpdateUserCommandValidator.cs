using BeautyGo.Application.Core.Errors;
using BeautyGo.Application.Core.Extensions;
using FluentValidation;

namespace BeautyGo.Application.Users.Commands.UpdateUser;

internal class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithError(ValidationErrors.CreateUser.FirstNameIsRequired);

        RuleFor(x => x.LastName).NotEmpty().WithError(ValidationErrors.CreateUser.LastNameIsRequired);
    }
}

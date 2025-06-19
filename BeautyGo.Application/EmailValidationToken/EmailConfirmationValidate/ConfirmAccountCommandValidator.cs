using BeautyGo.Application.Core.Errors;
using BeautyGo.Application.Core.Extensions;
using FluentValidation;

namespace BeautyGo.Application.EmailValidationToken.EmailConfirmationValidate;

internal class ConfirmAccountCommandValidator : AbstractValidator<ConfirmAccountCommand>
{
    public ConfirmAccountCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithError(ValidationErrors.CreateUser.FirstNameIsRequired);
    }
}

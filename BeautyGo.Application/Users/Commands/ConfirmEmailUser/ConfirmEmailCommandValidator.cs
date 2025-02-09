using BeautyGo.Application.Core.Errors;
using BeautyGo.Application.Core.Extensions;
using FluentValidation;

namespace BeautyGo.Application.Users.Commands.ConfirmEmailUser
{
    internal class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.Token).NotEmpty().WithError(ValidationErrors.CreateUser.FirstNameIsRequired);
        }
    }
}

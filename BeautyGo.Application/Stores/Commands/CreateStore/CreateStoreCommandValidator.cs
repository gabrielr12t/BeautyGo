using BeautyGo.Application.Core.Errors;
using BeautyGo.Application.Core.Extensions;
using BeautyGo.Domain.Helpers;
using FluentValidation;

namespace BeautyGo.Application.Stores.Commands.CreateStore;

internal class CreateStoreCommandValidator : AbstractValidator<CreateStoreCommand>
{
    public CreateStoreCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithError(ValidationErrors.CreateStore.NameIsRequired)
            .MinimumLength(3).WithError(ValidationErrors.CreateStore.NameInvalidLengthRequired);

        RuleFor(x => x.HomePageTitle)
            .NotEmpty().WithError(ValidationErrors.CreateStore.HomePageTitleIsRequired);

        RuleFor(x => x.HomePageDescription)
            .NotEmpty().WithError(ValidationErrors.CreateStore.HomePageDescriptionIsRequired);

        RuleFor(x => x.Description)
            .NotEmpty().WithError(ValidationErrors.CreateStore.DescriptionIsRequired);

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage(ValidationErrors.CreateStore.CnpjIsRequired)
            .Must(CommonHelper.IsValidCnpj).WithError(ValidationErrors.CreateStore.InvalidCnpj);

        RuleFor(x => x.AddressCep)
            .NotEmpty().WithMessage(ValidationErrors.CreateStore.CepIsRequired)
            .Must(CommonHelper.IsValidCEP).WithMessage(ValidationErrors.CreateStore.InvalidCep);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(ValidationErrors.CreateStore.PhoneIsRequired)
            .Matches(@"^\(\d{2}\) \d{5}-\d{4}$").WithMessage(ValidationErrors.CreateStore.InvalidPhoneFormat);
    }
}

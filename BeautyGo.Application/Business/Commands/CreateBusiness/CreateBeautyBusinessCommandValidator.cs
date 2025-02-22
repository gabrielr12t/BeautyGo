using BeautyGo.Application.Business.Commands.CreateBusiness;
using BeautyGo.Application.Core.Errors;
using BeautyGo.Application.Core.Extensions;
using BeautyGo.Domain.Helpers;
using FluentValidation;

namespace BeautyGo.Application.Business.Commands.CreateBusiness;

public class CreateBeautyBusinessCommandValidator : AbstractValidator<CreateBeautyBusinessCommand>
{
    public CreateBeautyBusinessCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithError(ValidationErrors.CreateBusiness.NameIsRequired)
            .MinimumLength(3).WithError(ValidationErrors.CreateBusiness.NameInvalidLengthRequired);

        RuleFor(x => x.HomePageTitle)
            .NotEmpty().WithError(ValidationErrors.CreateBusiness.HomePageTitleIsRequired);

        RuleFor(x => x.HomePageDescription)
            .NotEmpty().WithError(ValidationErrors.CreateBusiness.HomePageDescriptionIsRequired);

        RuleFor(x => x.Description)
            .NotEmpty().WithError(ValidationErrors.CreateBusiness.DescriptionIsRequired);

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage(ValidationErrors.CreateBusiness.CnpjIsRequired)
            .Must(CommonHelper.IsValidCnpj).WithError(ValidationErrors.CreateBusiness.InvalidCnpj);

        RuleFor(x => x.AddressCep)
            .NotEmpty().WithMessage(ValidationErrors.CreateBusiness.CepIsRequired)
            .Must(CommonHelper.IsValidCEP).WithMessage(ValidationErrors.CreateBusiness.InvalidCep);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(ValidationErrors.CreateBusiness.PhoneIsRequired)
            .Matches(@"^\(\d{2}\) \d{5}-\d{4}$").WithMessage(ValidationErrors.CreateBusiness.InvalidPhoneFormat);
    }
}

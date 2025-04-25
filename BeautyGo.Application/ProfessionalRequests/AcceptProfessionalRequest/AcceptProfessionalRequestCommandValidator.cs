using BeautyGo.Application.Core.Errors;
using BeautyGo.Application.Core.Extensions;
using FluentValidation;

namespace BeautyGo.Application.ProfessionalRequests.AcceptProfessionalRequest;

public class AcceptProfessionalRequestCommandValidator : AbstractValidator<AcceptProfessionalRequestCommand>
{
    public AcceptProfessionalRequestCommandValidator() =>
        RuleFor(p => p.ProfessionalRequestId)
            .NotEmpty()
            .WithError(ValidationErrors.AcceptProfessionalRequest.ProfessionalRequestIdIsRequired);
}

using BeautyGo.Application.Core.Errors;
using BeautyGo.Application.Core.Extensions;
using FluentValidation;

namespace BeautyGo.Application.Businesses.Commands.CreateWorkingHours;

public class CreateWorkingHoursCommandValidator : AbstractValidator<CreateWorkingHoursCommand>
{
    public CreateWorkingHoursCommandValidator()
    {
        RuleFor(c => c.WorkingHours)
            .NotEmpty().WithError(ValidationErrors.CreateWorkingHours.InvalidDayOfWeek)
            .Must(wh => wh.Count <= 7).WithError(ValidationErrors.CreateWorkingHours.ProvideForUpToSevenDays);

        RuleForEach(x => x.WorkingHours)
            .ChildRules(workingHours =>
            {
                workingHours.RuleFor(x => x.DayOfWeek)
                    .IsInEnum().WithError(ValidationErrors.CreateWorkingHours.InvalidDayOfWeek);

                workingHours.RuleFor(x => x.OpeningTime)
                    .GreaterThan(TimeSpan.Zero).WithError(ValidationErrors.CreateWorkingHours.InvalidOpeningTime);

                workingHours.RuleFor(x => x.ClosingTime)
                    .GreaterThan(TimeSpan.Zero).WithError(ValidationErrors.CreateWorkingHours.InvalidClosingTime)
                    .GreaterThan(x => x.OpeningTime).WithError(ValidationErrors.CreateWorkingHours.ClosingTimeNotMustBeAfterOpeningTime);
            });
    }
}

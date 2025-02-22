using BeautyGo.Domain.Core.Primitives;
using FluentValidation.Results;

namespace BeautyGo.Domain.Core.Exceptions;

public sealed class CustomValidationException : Exception
{
    public CustomValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures has occurred.") =>
        Errors = failures
            .Distinct()
            .Select(failure => new Error(failure.ErrorCode, failure.ErrorMessage))
            .ToList();

    public IReadOnlyCollection<Error> Errors { get; }
}

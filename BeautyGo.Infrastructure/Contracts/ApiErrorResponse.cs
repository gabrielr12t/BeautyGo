using BeautyGo.Domain.Core.Primitives;

namespace BeautyGo.Infrastructure.Contracts;

public record ApiErrorResponse(IReadOnlyCollection<Error> Errors);

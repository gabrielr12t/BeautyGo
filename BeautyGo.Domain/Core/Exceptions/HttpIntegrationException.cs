using BeautyGo.Domain.Core.Primitives;

namespace BeautyGo.Domain.Core.Exceptions;

public class HttpIntegrationException : Exception
{
    public HttpIntegrationException(IReadOnlyCollection<Error> error)
        : base(string.Join(Environment.NewLine, error.Select(p => p.Message)))
        => Errors = error;

    public HttpIntegrationException()
    {

    }

    public IReadOnlyCollection<Error> Errors { get; }
}

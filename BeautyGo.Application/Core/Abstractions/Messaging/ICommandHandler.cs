using MediatR;

namespace BeautyGo.Application.Core.Abstractions.Messaging;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}

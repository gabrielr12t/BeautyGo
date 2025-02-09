using MediatR;

namespace BeautyGo.Application.Core.Abstractions.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

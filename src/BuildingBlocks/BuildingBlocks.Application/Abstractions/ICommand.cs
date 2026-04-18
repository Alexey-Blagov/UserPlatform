using MediatR;

namespace BuildingBlocks.Application.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

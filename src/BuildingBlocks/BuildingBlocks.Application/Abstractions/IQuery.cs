using MediatR;

namespace BuildingBlocks.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}

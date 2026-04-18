using AdminService.Contracts.Dtos;
using BuildingBlocks.Application.Common;
using MediatR;

namespace AdminService.Application.Queries.GetSystemSummary;

public sealed class GetSystemSummaryQueryHandler : IRequestHandler<GetSystemSummaryQuery, Result<SystemSummaryDto>>
{
    public Task<Result<SystemSummaryDto>> Handle(GetSystemSummaryQuery request, CancellationToken cancellationToken)
    {
        var dto = new SystemSummaryDto(
            "AdminService",
            DateTime.UtcNow,
            new[]
            {
                "Protected admin endpoints",
                "Keycloak role based access",
                "Clean Architecture skeleton",
                "Ready for audit, metrics and management commands"
            });

        return Task.FromResult(Result<SystemSummaryDto>.Success(dto));
    }
}

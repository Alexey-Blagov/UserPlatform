using AdminService.Contracts.Dtos;
using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Common;

namespace AdminService.Application.Queries.GetSystemSummary;

public sealed record GetSystemSummaryQuery() : IQuery<Result<SystemSummaryDto>>;

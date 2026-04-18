namespace AdminService.Contracts.Dtos;

public sealed record SystemSummaryDto(
    string ServiceName,
    DateTime GeneratedAtUtc,
    string[] Features);

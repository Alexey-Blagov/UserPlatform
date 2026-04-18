namespace BuildingBlocks.Application.Abstractions;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    string? Subject { get; }
    string? UserName { get; }
    string? Email { get; }
    IReadOnlyCollection<string> Roles { get; }
}

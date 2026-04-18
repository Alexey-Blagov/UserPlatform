using System.Security.Claims;
using BuildingBlocks.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Infrastructure.Security;

public sealed class HttpContextCurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public string? Subject =>
        User?.FindFirst("sub")?.Value ??
        User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? UserName =>
        User?.FindFirst("preferred_username")?.Value ??
        User?.Identity?.Name;

    public string? Email =>
        User?.FindFirst("email")?.Value ??
        User?.FindFirst(ClaimTypes.Email)?.Value;

    public IReadOnlyCollection<string> Roles =>
        User?.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray()
        ?? Array.Empty<string>();
}

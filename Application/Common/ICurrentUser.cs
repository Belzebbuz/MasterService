using System.Security.Claims;
using Application.Common.Contracts;

namespace Application.Common;

public interface ICurrentUser : IScopedService
{
    string? Name { get; }

    Guid GetUserId();

    string? GetUserEmail();

    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();
}

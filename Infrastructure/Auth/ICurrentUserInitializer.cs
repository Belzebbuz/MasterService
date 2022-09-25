using Application.Common.Contracts;
using System.Security.Claims;

namespace Infrastructure.Auth;

public interface ICurrentUserInitializer : IScopedService
{
	void SetCurrentUser(ClaimsPrincipal user);

	void SetCurrentUserId(string userId);
}

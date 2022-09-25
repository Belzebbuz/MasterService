using System.Security.Claims;

namespace Infrastructure.Auth
{
	public static class ClaimsPrincipalExtensions
	{
		public static string? GetUserId(this ClaimsPrincipal principal)
		=> principal.FindFirstValue(ClaimTypes.NameIdentifier);

		public static string? GetEmail(this ClaimsPrincipal principal)
		=> principal.FindFirstValue(ClaimTypes.Email);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebUI.Application.Extensions;

    public static class ClaimPrincipalsExtensions
    {
		public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
			=> claimsPrincipal.FindFirstValue(ClaimTypes.Email);

		public static string GetFullName(this ClaimsPrincipal claimsPrincipal)
				=> claimsPrincipal.FindFirstValue(ClaimTypes.Name);

		public static string GetPhoneNumber(this ClaimsPrincipal claimsPrincipal)
				=> claimsPrincipal.FindFirstValue(ClaimTypes.MobilePhone);

		public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
			   => claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

	public static string FindFirstValue(this ClaimsPrincipal principal, string claimType)
	{
		if (principal == null)
		{
			throw new ArgumentNullException(nameof(principal));
		}
		var claim = principal.FindFirst(claimType);
		return claim != null ? claim.Value : null;
	}
}



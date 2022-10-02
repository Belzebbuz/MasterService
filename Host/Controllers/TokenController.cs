using Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Messages.Identity;
using Shared.Wrapper;

namespace Host.Controllers;

[Route("api/identity/token")]
public class TokenController : BaseApiController
{
	private readonly ITokenService _tokenService;

	public TokenController(ITokenService tokenService)
	{
		_tokenService = tokenService;
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<IResult<IDR_002>> GetTokenAsync(IDM_002 request, CancellationToken cancellationToken)
	{
		return await _tokenService.GetTokenAsync(request, GetIpAddress(), cancellationToken);
	}

	private string GetIpAddress() =>
	   Request.Headers.ContainsKey("X-Forwarded-For")
		   ? Request.Headers["X-Forwarded-For"]
		   : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
}

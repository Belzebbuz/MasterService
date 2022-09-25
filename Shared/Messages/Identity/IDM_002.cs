using MediatR;
using Shared.Wrapper;

namespace Shared.Messages.Identity;

public class IDM_002 : IRequest<IResult<IDR_002>>
{
	public string Email { get; set; }
	public string Password { get; set; }
}

public class IDR_002
{
	public string Token { get; set; }
	public string RefreshToken { get; set; }
	public DateTime RefreshTokenExpiryTime { get; set; }
}
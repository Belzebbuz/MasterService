using MediatR;
using Shared.Wrapper;

namespace Shared.Messages.Identity;

public class IDM_001 : IRequest<IResult>
{
	public string FullName { get; set; } = default!;
	public string Email { get; set; } = default!;
	public string UserName { get; set; } = default!;
	public string Password { get; set; } = default!;
	public string ConfirmPassword { get; set; } = default!;
	public string? PhoneNumber { get; set; }
}

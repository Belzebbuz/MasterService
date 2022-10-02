using Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Messages.Identity;
using Shared.Wrapper;
using IResult = Shared.Wrapper.IResult;
namespace Host.Controllers;

[Route("api/identity/user")]
public class UserController : BaseApiController
{
	private readonly IUserService _userService;

	public UserController(IUserService userService)
	{
		_userService = userService;
	}

	[HttpPost("self-register")]
	[AllowAnonymous]
	public async Task<IResult> SelfRegisterAsync(IDM_001 message)
	 => await _userService.CreateUserAsync(message);

	[HttpGet("{id}")]
	[Authorize]
	public async Task<IResult<IDR_003>> GetByIdAsync(string id)
	 => await _userService.GetUserAsync(new IDM_003(id));
}

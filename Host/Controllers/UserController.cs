using Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Messages.Identity;
using IResult = Shared.Wrapper.IResult;
namespace Host.Controllers;

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

}

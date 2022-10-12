using Application.Common;
using Domain.Models.Constants;
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

	[HttpPost]
	public async Task<IResult> RegisterAsync(IDM_001 message)
	 => await _userService.CreateUserAsync(message);

	[HttpGet("{id}")]
	[Authorize]
	public async Task<IResult<IDR_003>> GetByIdAsync(string id)
	 => await _userService.GetUserAsync(new IDM_003(id));

	[HttpGet]
	[Authorize(Roles = UserRoles.Admin)]
	public async Task<IResult<IDR_004>> GetAllAsync()
	 => await _userService.GetAllAsync();
}

using Application.Common;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Messages.Identity;
using Shared.Wrapper;
using System;
using System.Threading.Tasks;
using IResult = Shared.Wrapper.IResult;

namespace Host.Controllers;

[Route("api/identity/account")]
[Authorize]
public class AccountController : BaseApiController
{
	private readonly IAccountService _accountService;
	private readonly ICurrentUser _currentUser;

	public AccountController(IAccountService accountService, ICurrentUser currentUser)
	{
		_accountService = accountService;
		_currentUser = currentUser;
	}

	[HttpGet("profile-picture/{userId}")]
	public async Task<IResult<string>> GetProfilePictureAsync(string userId)
		=> await _accountService.GetProfilePictureAsync(userId);

	[HttpPut(nameof(UpdateProfile))]
	public async Task<IResult> UpdateProfile(IDM_006 request) 
		=> await _accountService.UpdateProfileAsync(request, _currentUser.UserId);

	[HttpPost("profile-picture/{userId}")]
	public async Task<IResult<string>> UpdateProfilePictureAsync(IDM_007 request) 
		=> await _accountService.UpdateProfilePictureAsync(request, _currentUser.UserId);

	[HttpPut(nameof(ChangePassword))]
	public async Task<IResult> ChangePassword(IDM_008 request)
		=> await _accountService.ChangePasswordAsync(request, _currentUser.UserId);
}

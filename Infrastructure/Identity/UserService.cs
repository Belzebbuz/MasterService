using Application.Common;
using Domain.Models;
using Domain.Models.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Messages.Identity;
using Shared.Wrapper;

namespace Infrastructure.Identity;

public class UserService : IUserService
{
	private readonly UserManager<AppUser> _userManager;

	public UserService(UserManager<AppUser> userManager)
	{
		_userManager = userManager;
	}
	public async Task<IResult> CreateUserAsync(IDM_001 message)
	{
		var user = new AppUser
		{
			Email = message.Email,
			FullName = message.FullName,
			UserName = message.UserName,
			PhoneNumber = message.PhoneNumber,
			IsActive = true
		};

		var result = await _userManager.CreateAsync(user, message.Password);
		if (!result.Succeeded)
		{
			return await Result.FailAsync(result.Errors.Select(x => x.Description).ToList());
		}

		await _userManager.AddToRoleAsync(user, UserRoles.Basic);
		return await Result.SuccessAsync();
	}
}

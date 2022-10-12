using Application.Common;
using Domain.Models;
using Domain.Models.Constants;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Messages.Identity;
using Shared.Wrapper;

namespace Infrastructure.Identity;

public class UserService : IUserService
{
	private readonly UserManager<AppUser> _userManager;
	private readonly IMapper _mapper;
	private readonly IConfiguration _configuration;

	public UserService(UserManager<AppUser> userManager, IMapper mapper, IConfiguration configuration)
	{
		_userManager = userManager;
		_mapper = mapper;
		_configuration = configuration;
	}
	public async Task<IResult> CreateUserAsync(IDM_001 message)
	{
		var user = new AppUser
		{
			Email = message.Email,
			FullName = message.FullName,
			UserName = message.Email,
			PhoneNumber = message.PhoneNumber,
			IsActive = true
		};

		var result = await _userManager.CreateAsync(user, message.Password);
		if (!result.Succeeded)
		{
			return await Result.FailAsync(result.Errors.Select(x => x.Description).ToList());
		}

		await _userManager.AddToRoleAsync(user, UserRoles.Basic);
		if(user.Email == _configuration.GetValue<string>("SecuritySettings:RootAdmin"))
		{
			await _userManager.AddToRoleAsync(user, UserRoles.Admin);
			await _userManager.AddToRoleAsync(user, UserRoles.Master);
		}
		return await Result.SuccessAsync($"Пользователь {user.Email} успешно создан!");
	}

	public async Task<IResult<IDR_004>> GetAllAsync()
	{
		var users = await _userManager.Users.ToListAsync();
		var result = _mapper.Map<List<UserResponse>>(users);
		return await Result<IDR_004>.SuccessAsync(new IDR_004(result));
	}

	public async Task<IResult<IDR_003>> GetUserAsync(IDM_003 message)
	{
		var user = await _userManager.FindByIdAsync(message.UserId);
		if (user == null)
		{
			return await Result<IDR_003>.FailAsync("Пользователь не найден!");
		}
		else
		{
			var userDTO = _mapper.Map<IDR_003>(user);
			return await Result<IDR_003>.SuccessAsync(userDTO);
		}
	}
}

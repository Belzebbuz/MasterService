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

namespace Infrastructure.Services.Identity;

public class UserService : IUserService
{
	private readonly UserManager<AppUser> _userManager;
	private readonly IMapper _mapper;
	private readonly string _rootUser;
	private readonly RoleManager<IdentityRole> _roleManager;

	public UserService(UserManager<AppUser> userManager,
		IMapper mapper,
		IConfiguration configuration,
		RoleManager<IdentityRole> roleManager)
	{
		_userManager = userManager;
		_mapper = mapper;
		_rootUser = configuration.GetValue<string>("SecuritySettings:RootAdmin");
		_roleManager = roleManager;
	}
	public async Task<IResult> CreateUserAsync(IDM_001 message)
	{
		var user = AppUser.Create(message.FullName, message.Email, message?.PhoneNumber);

		var result = await _userManager.CreateAsync(user, message.Password);
		if (!result.Succeeded)
		{
			return await Result.FailAsync(result.Errors.Select(x => x.Description).ToList());
		}

		await _userManager.AddToRoleAsync(user, UserRoles.Basic);
		if (user.Email == _rootUser)
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
			return await Result<IDR_003>.FailAsync("Пользователь не найден!");

		var userDTO = _mapper.Map<IDR_003>(user);
		await AddRolesToDto(user, userDTO);
		return await Result<IDR_003>.SuccessAsync(userDTO);
	}

	public async Task<IResult> ToggleUserStatusAsync(IDM_009 message)
	{
		var user = await _userManager.Users.Where(u => u.Id == message.UserId).FirstOrDefaultAsync();
		var isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
		if (isAdmin)
		{
			return await Result.FailAsync("Невозможно изменить активность администратора");
		}
		if (user != null)
		{
			user.SetIsActiveStatus(message.ActivateUser);
			var identityResult = await _userManager.UpdateAsync(user);
		}
		return await Result.SuccessAsync();
	}

	public async Task<IResult> UpdateRolesAsync(IDM_005 message)
	{
		var user = await _userManager.FindByIdAsync(message.UserId);
		if (user.Email == _rootUser)
			return await Result.FailAsync("Недоступно для данного пользователя");

		var roles = await _userManager.GetRolesAsync(user);
		var selectedRoles = message.UserRoles.Where(x => x.Selected).ToList();

		var result = await _userManager.RemoveFromRolesAsync(user, roles);
		result = await _userManager.AddToRolesAsync(user, selectedRoles.Select(y => y.Name));
		return await Result.SuccessAsync("Роли обновлены");
	}

	private async Task AddRolesToDto(AppUser user, IDR_003 userDTO)
	{
		var allRoles = await _roleManager.Roles.ToListAsync();
		userDTO.Roles = new();
		foreach (var role in allRoles)
		{
			var userRoleModel = new UserRoleModel()
			{
				Name = role.Name
			};
			if (await _userManager.IsInRoleAsync(user, role.Name))
			{
				userRoleModel.Selected = true;
			}
			else
			{
				userRoleModel.Selected = false;
			}
			userDTO.Roles.Add(userRoleModel);
		}
	}
}

using Application.Common;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Messages.Identity;
using Shared.Wrapper;

namespace Infrastructure.Services.Identity;

public class AccountService : IAccountService
{
	private readonly UserManager<AppUser> _userManager;
	private readonly SignInManager<AppUser> _signInManager;
	private readonly IUploadService _uploadService;

	public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IUploadService uploadService)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_uploadService = uploadService;
	}

	public async Task<IResult> ChangePasswordAsync(IDM_008 request, string userId)
	{
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
		{
			return await Result.FailAsync("Пользователь не найден");
		}

		var identityResult = await this._userManager.ChangePasswordAsync(
			user,
			request.Password,
			request.NewPassword);
		var errors = identityResult.Errors.Select(e => e.Description.ToString()).ToList();
		return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
	}

	public async Task<Result<FileStream>> GetProfilePictureAsync(string userId)
	{
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			return await Result<FileStream>.FailAsync("Пользователь не найден");
		if (user.ProfilePictureDataUrl == null)
			return await Result<FileStream>.FailAsync("Изображение отсутствует");
		//var bytes = File.ReadAllBytes(user.ProfilePictureDataUrl);
		//var baseString = Convert.ToBase64String(bytes, 0, bytes.Length);
		return await Result<FileStream>.SuccessAsync(data: new FileStream(user.ProfilePictureDataUrl, FileMode.Open, FileAccess.Read));
	}

	public async Task<IResult<string>> GetProfilePictureUrlAsync(string userId)
	{
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			return await Result<string>.FailAsync("Пользователь не найден");
		if (user.ProfilePictureDataUrl == null)
			return await Result<string>.FailAsync("Изображение отсутствует");

		return await Result<string>.SuccessAsync(data: user.ProfilePictureDataUrl);
	}

	public async Task<IResult> UpdateProfileAsync(IDM_006 request, string userId)
	{
		if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
		{
			var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
			if (userWithSamePhoneNumber != null)
			{
				return await Result.FailAsync(string.Format("Этот номер телефона уже используется", request.PhoneNumber));
			}
		}

		var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
		if (userWithSameEmail == null || userWithSameEmail.Id == userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return await Result.FailAsync("Пользователь не найден");
			}
			user.Update(request.FullName, request.PhoneNumber);
			var identityResult = await _userManager.UpdateAsync(user);
			var errors = identityResult.Errors.Select(e => e.Description).ToList();
			await _signInManager.RefreshSignInAsync(user);
			return identityResult.Succeeded ? await Result.SuccessAsync("Профиль успешно обновлен") : await Result.FailAsync(errors);
		}
		else
		{
			return await Result.FailAsync(string.Format("E-mail уже используется", request.Email));
		}
	}

	public async Task<IResult<string>> UpdateProfilePictureAsync(IDM_007 request, string userId)
	{
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null) return await Result<string>.FailAsync("Пользователь не найден");
		string filePath = _uploadService.Upload(request);

		if(!string.IsNullOrEmpty(user.ProfilePictureDataUrl)) File.Delete(user.ProfilePictureDataUrl);

		user.SetProfilePictureFilePath(filePath);
		var identityResult = await _userManager.UpdateAsync(user);
		var errors = identityResult.Errors.Select(e => e.Description).ToList();

		if(!string.IsNullOrEmpty(user.ProfilePictureDataUrl))
		{
			var bytes = File.ReadAllBytes(user.ProfilePictureDataUrl);
			var baseString = Convert.ToBase64String(bytes, 0, bytes.Length);
			return await Result<string>.SuccessAsync(baseString, "Изображение обновлено");
		}

		return identityResult.Succeeded ? await Result<string>.SuccessAsync("Изображение удалено") : await Result<string>.FailAsync(errors);
	}
}

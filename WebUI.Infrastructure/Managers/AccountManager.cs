using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Routing;
using Shared.Messages.Identity;
using Shared.Wrapper;
using System.Net.Http.Json;
using System.Reflection;
using WebUI.Application.Services.Managers;
using WebUI.Infrastructure.Constants;
using WebUI.Infrastructure.Endpoints;

namespace WebUI.Infrastructure.Managers;

public class AccountManager : IAccountManager
{
	private readonly HttpClient _httpClient;
	private readonly ILocalStorageService _localStorageService;

	public AccountManager(HttpClient httpClient, ILocalStorageService localStorageService)
	{
		_httpClient = httpClient;
		_localStorageService = localStorageService;
	}

	public async Task<IResult> ChangePasswordAsync(IDM_008 model)
	{
		var response = await _httpClient.PutAsJsonAsync(AccountEndpoints.ChangePassword, model);
		return await response.ToResult();
	}

	public async Task<IResult<string>> GetProfilePictureAsync(string userId)
	{
		var response = await _httpClient.GetAsync(AccountEndpoints.GetProfilePicture(userId));
		return await response.ToResult<string>();
	}

	public async Task<IResult> UpdateProfileAsync(IDM_006 profileModel)
	{
		var response = await _httpClient.PutAsJsonAsync(AccountEndpoints.UpdateProfile, profileModel);
		return await response.ToResult();
	}

	public async Task<IResult<string>> UpdateProfilePictureAsync(IDM_007 request, string userId)
	{
		var response = await _httpClient.PostAsJsonAsync(AccountEndpoints.UpdateProfilePicture(userId), request);
		return await response.ToResult<string>();
	}

	public async Task<string> GetCurrentUserProfileImageAsync(string userId)
	{
		var imageData = await GetProfilePictureAsync(userId);
		if (imageData.Succeeded)
		{
			return $"data:image/jpg;base64,{imageData.Data}";
		}
		return string.Empty;
	}
}

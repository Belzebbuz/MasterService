using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Messages.Identity;
using Shared.Wrapper;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Claims;
using WebUI.Application.Services.Managers;
using WebUI.Infrastructure.Authentication;
using WebUI.Infrastructure.Constants;
using WebUI.Infrastructure.Endpoints;

namespace WebUI.Infrastructure.Managers;

public class AuthenticationManager : IAuthenticationManager
{
	private readonly HttpClient _httpClient;
	private readonly ILocalStorageService _localStorage;
	private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthenticationManager(
        HttpClient httpClient, 
        ILocalStorageService localStorage, 
        AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<ClaimsPrincipal> CurrentUser()
    {
		var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
		return state.User;
	}
	public async Task<IResult> Login(IDM_002 tokenRequest)
    {
		try
		{
			var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get, tokenRequest);
			var result = await response.ToResult<IDR_002>();
			if (result.Succeeded)
			{
				var token = result.Data.Token;
				var refreshToken = result.Data.RefreshToken;
				await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
				await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);

				((WebAuthStateProvider)this._authenticationStateProvider).MarkUserAsAuthenticated(tokenRequest.Email);
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				return await Result.SuccessAsync();
			}
			else
			{
				return await Result.FailAsync(result.Messages);
			}
		}
		catch (Exception ex)
		{
			return await Result.FailAsync(ex.GetBaseException().Message);
		}

	}

    public async Task<IResult> Logout()
    {
		await _localStorage.RemoveItemAsync(StorageConstants.Local.AuthToken);
		await _localStorage.RemoveItemAsync(StorageConstants.Local.RefreshToken);
		((WebAuthStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
		_httpClient.DefaultRequestHeaders.Authorization = null;
		return await Result.SuccessAsync();
	}
}

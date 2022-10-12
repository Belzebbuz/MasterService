using MediatR;
using Microsoft.AspNetCore.Components.Routing;
using Shared.Messages.Identity;
using Shared.Wrapper;
using System.Net.Http.Json;
using WebUI.Application.Services.Managers;
using WebUI.Infrastructure.Endpoints;

namespace WebUI.Infrastructure.Managers;

public class UserManager : IUserManager
{
	private readonly HttpClient _httpClient;

	public UserManager(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}
	public async Task<IResult<IDR_003>> GetAsync(string userId)
	{
		var response = await _httpClient.GetAsync(UserEndpoints.Get(userId));
		return await response.ToResult<IDR_003>();
	}

	public async Task<IResult<List<UserResponse>>> GetAllAsync()
	{
		var response = await _httpClient.GetAsync(UserEndpoints.GetAll);
		var users = await response.ToResult<IDR_004>();
		return await Result<List<UserResponse>>.SuccessAsync(users.Data.Users);
	}

	public async Task<IResult> RegisterUserAsync(IDM_001 registerRequest)
	{
		var response = await _httpClient.PostAsJsonAsync(UserEndpoints.Register, registerRequest);
		return await response.ToResult();
	}
}

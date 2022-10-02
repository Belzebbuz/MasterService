using Microsoft.AspNetCore.Components.Routing;
using Shared.Messages.Identity;
using Shared.Wrapper;
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
}

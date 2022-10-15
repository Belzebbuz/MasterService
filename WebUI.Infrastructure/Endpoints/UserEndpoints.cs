namespace WebUI.Infrastructure.Endpoints;

public static class UserEndpoints
{
	public static string GetAll = "api/identity/user";
	public static string Register = "api/identity/user";
	public static string ToggleUserStatus = "api/identity/user/toggle-status";
	public static string Get(string userId)
	{
		return $"api/identity/user/{userId}";
	}
	public static string GetUserRoles(string userId)
	{
		return $"api/identity/user/roles/{userId}";
	}
}

namespace WebUI.Infrastructure.Endpoints;

public class AccountEndpoints
{
	public const string ChangePassword = "api/identity/account/changepassword";
	public const string UpdateProfile = "api/identity/account/updateprofile";

	public static string GetProfilePicture(string userId)
	{
		return $"api/identity/account/profile-picture/{userId}";
	}

	public static string UpdateProfilePicture(string userId)
	{
		return $"api/identity/account/profile-picture/{userId}";
	}
}

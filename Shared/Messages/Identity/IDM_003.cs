namespace Shared.Messages.Identity;

public class IDM_003
{
	public string UserId { get; private set; }

	public IDM_003(string userId)
	{
		UserId = userId;
	}
}
public class IDR_003
{
	public IDR_003(string userId, string fullName, string email, string? phoneNumber = null)
	{
		UserId = userId;
		FullName = fullName;
		Email = email;
		PhoneNumber = phoneNumber;
	}

	public string UserId { get; private set; }
	public string FullName { get; private set; }
	public string Email { get; private set; }
	public string? PhoneNumber { get; private set; }
}

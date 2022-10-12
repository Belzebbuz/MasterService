namespace Shared.Messages.Identity;

public class IDR_004
{
	public List<UserResponse> Users { get; private set; }

	public IDR_004(List<UserResponse> users)
	{
		Users = users;
	}
}
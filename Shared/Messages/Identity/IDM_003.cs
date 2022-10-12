namespace Shared.Messages.Identity;

public class IDM_003
{
	public string UserId { get; private set; }

	public IDM_003(string userId)
	{
		UserId = userId;
	}
}

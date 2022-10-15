namespace Shared.Messages.Identity;
/// <summary>
/// Список пользователей
/// </summary>
public class IDR_004
{
	public List<UserResponse> Users { get; private set; }

	public IDR_004(List<UserResponse> users)
	{
		Users = users;
	}
}
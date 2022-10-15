namespace Shared.Messages.Identity;

/// <summary>
/// Запрос на обновление ролей пользователя
/// </summary>
public class IDM_005
{
	public IDM_005(string userId, IList<UserRoleModel> userRoles)
	{
		UserId = userId;
		UserRoles = userRoles;
	}

	public string UserId { get; private set; }
	public IList<UserRoleModel> UserRoles { get; private set; }
}

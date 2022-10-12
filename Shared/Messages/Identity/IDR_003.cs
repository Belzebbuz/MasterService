namespace Shared.Messages.Identity;

public class IDR_003 : UserResponse
{
	public List<UserRoleModel> Roles { get; set; }
}

public class UserRoleModel
{
	public string Name { get; set; }
	public bool Selected { get; set; }
}
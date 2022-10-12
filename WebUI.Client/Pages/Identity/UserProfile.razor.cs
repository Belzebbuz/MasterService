using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Messages.Identity;
using Shared.Wrapper;

namespace WebUI.Client.Pages.Identity;

public partial class UserProfile
{
	[Parameter] public string Id { get; set; }
	[Parameter] public string Title { get; set; }
	[Parameter] public string Description { get; set; }
	public List<UserRoleModel> UserRolesList { get; set; } = new();

	private char _firstLetterOfName;
	private string _userId;
	private string _fullName;
	private string _phoneNumber;
	private string _email;

	private bool _loaded;

	protected override async Task OnInitializedAsync()
	{
		var userId = Id;
		var result = await _userManager.GetAsync(userId);
		if (result.Succeeded)
		{
			var user = result.Data;
			if (user != null)
			{
				_userId = user.Id;
				_fullName = user.FullName;
				_email = user.Email;
				_phoneNumber = user.PhoneNumber;
				UserRolesList = user.Roles;
			}
			Title = $"Профиль {_fullName}";
			Description = _email;
			if (_fullName.Length > 0)
			{
				_firstLetterOfName = _fullName[0];
			}
		}

		_loaded = true;
	}

	private async Task SaveRolesAsync()
	{
		var request = new IDM_005(_userId, UserRolesList);
		IResult result = await _userManager.UpdateRolesAsync(request);
		if (result.Succeeded)
		{
			_snackBar.Add(result.Messages[0], Severity.Success);
			_navigationManager.NavigateTo("/identity/users");
		}
		else
		{
			foreach (var error in result.Messages)
			{
				_snackBar.Add(error, Severity.Error);
			}
		}
	}
}
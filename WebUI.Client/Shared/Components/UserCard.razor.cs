using Microsoft.AspNetCore.Components;
using WebUI.Application.Extensions;

namespace WebUI.Client.Shared.Components;

public partial class UserCard
{
	[Parameter] public string Class { get; set; }
	private string FullName { get; set; }
	private string Email { get; set; }
	private char FirstLetterOfName { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await LoadDataAsync();
	}

	private async Task LoadDataAsync()
	{
		var state = await _stateProvider.GetAuthenticationStateAsync();
		var user = state.User;

		Email = user.GetEmail();
		FullName = user.GetFullName();
		if (this.FullName.Length > 0)
		{
			FirstLetterOfName = FullName[0];
		}
	}
}
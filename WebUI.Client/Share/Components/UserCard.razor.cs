using Microsoft.AspNetCore.Components;
using WebUI.Application.Extensions;
using WebUI.Client.Pages.Identity;
using WebUI.Infrastructure.Constants;

namespace WebUI.Client.Share.Components;

public partial class UserCard
{
    [Parameter] public string Class { get; set; }
    private string FullName { get; set; }
    private string Email { get; set; }
    private char FirstLetterOfName { get; set; }
	[Parameter]
	public string ImageDataUrl { get; set; }
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
        ImageDataUrl = await _accountManager.GetCurrentUserProfileImageAsync(user.GetUserId());
		if (FullName.Length > 0)
		{
			FirstLetterOfName = FullName[0];
		}
	}
}
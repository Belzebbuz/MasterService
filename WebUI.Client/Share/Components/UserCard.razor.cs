using Microsoft.AspNetCore.Components;
using WebUI.Application.Extensions;

namespace WebUI.Client.Share.Components;

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
        if (FullName.Length > 0)
        {
            FirstLetterOfName = FullName[0];
        }
    }
}
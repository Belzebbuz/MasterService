using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebUI.Application.Extensions;
using WebUI.Client.Pages.Identity;
using WebUI.Client.Settings;

namespace WebUI.Client.Share
{
    public partial class MainLayout
    {
        private string _fullName;
        private string _email;
        private string _userId;
        private char _firstLetterOfName;
        private MudTheme _currentTheme = UITheme.DefaultTheme;
        private bool _drawerOpen = true;
		[Parameter]
		public string ImageDataUrl { get; set; }
		private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user == null) return;
            if (user.Identity?.IsAuthenticated == true)
            {
                _userId = user.GetUserId();
                _fullName = user.GetFullName();
                _email = user.GetEmail();
				ImageDataUrl = await _accountManager.GetCurrentUserProfileImageAsync(user.GetUserId());
				if (_fullName.Length > 0)
				{
					_firstLetterOfName = _fullName[0];
				}
				var currentUserResult = await _userManager.GetAsync(_userId);
                if (!currentUserResult.Succeeded || currentUserResult.Data == null)
                {
                    _snackBar.Add("You are logged out because the user with your Token has been deleted.", Severity.Error);
                    await _authenticationManager.Logout();
                }
            }
        }
        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }
        private void Logout()
        {
            var parameters = new DialogParameters
            {
                {nameof(Dialogs.Logout.ButtonText), "Выход"},
                {nameof(Dialogs.Logout.Color), Color.Error}
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            _dialogService.Show<Dialogs.Logout>("Выход", parameters, options);
        }
    }
}
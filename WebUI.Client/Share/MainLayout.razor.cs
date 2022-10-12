using MudBlazor;
using WebUI.Application.Extensions;
using WebUI.Client.Settings;

namespace WebUI.Client.Share
{
    public partial class MainLayout
    {
        private string FullName { get; set; }
        private string Email { get; set; }
        private string CurrentUserId { get; set; }
        private char FirstLetterOfName { get; set; }
        private MudTheme _currentTheme = UITheme.DefaultTheme;
        private bool _drawerOpen = true;
        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user == null) return;
            if (user.Identity?.IsAuthenticated == true)
            {
                CurrentUserId = user.GetUserId();
                FullName = user.GetFullName();
                Email = user.GetEmail();
                if (FullName.Length > 0)
                {
                    FirstLetterOfName = FullName[0];
                }
                var currentUserResult = await _userManager.GetAsync(CurrentUserId);
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
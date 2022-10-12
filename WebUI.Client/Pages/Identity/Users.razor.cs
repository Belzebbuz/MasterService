
using MudBlazor;
using Shared.Messages.Identity;

namespace WebUI.Client.Pages.Identity
{
	public partial class Users
	{
		private List<UserResponse> _usersList = new();
		private bool _loaded;
		private string _searchString = "";
		protected override async Task OnInitializedAsync()
		{
			await GetUsersAsync();
			_loaded = true;
		}

		private async Task GetUsersAsync()
		{
			var response = await _userManager.GetAllAsync();
			if (response.Succeeded)
			{
				_usersList = response.Data;
			}
			else
			{
				foreach (var message in response.Messages)
				{
					_snackBar.Add(message, Severity.Error);
				}
			}
		}
		private bool Search(UserResponse user)
		{
			if (string.IsNullOrWhiteSpace(_searchString)) return true;
			if (user.FullName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
			{
				return true;
			}
			if (user.Email?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
			{
				return true;
			}
			if (user.PhoneNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
			{
				return true;
			}
			return false;
		}

		private async Task InvokeNewUserDialog()
		{
			var parameters = new DialogParameters();
			var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
			var dialog = _dialogService.Show<RegisterUserDialog>("Новый пользователь", parameters, options);
			var result = await dialog.Result;
			if (!result.Cancelled)
			{
				await GetUsersAsync();
			}
		}
		private void ViewProfile(string userId)
		{
			_navigationManager.NavigateTo($"/identity/user-profile/{userId}");
		}
	}
}
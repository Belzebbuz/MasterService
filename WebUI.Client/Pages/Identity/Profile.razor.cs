using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Shared.Messages.Identity;
using Shared.Wrapper;
using WebUI.Application.Extensions;
using WebUI.Client.Share.Dialogs;
using WebUI.Infrastructure.Constants;

namespace WebUI.Client.Pages.Identity;

public partial class Profile
{
	private FluentValidationValidator? _fluentValidationValidator;
	private bool Validated => _fluentValidationValidator!.Validate(options => { options.IncludeAllRuleSets(); });
	private char _firstLetterOfName;
	private readonly IDM_006 _profileModel = new();
	private IBrowserFile _file;
	public string UserId { get; set; }

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
		_profileModel.Email = user.GetEmail();
		_profileModel.FullName = user.GetFullName();
		_profileModel.PhoneNumber = user.GetPhoneNumber();
		UserId = user.GetUserId();
		ImageDataUrl = await _accountManager.GetCurrentUserProfileImageAsync(user.GetUserId());
		if (_profileModel.FullName.Length > 0)
		{
			_firstLetterOfName = _profileModel.FullName[0];
		}
	}

	private async Task UpdateProfileAsync()
	{
		IResult response = await _accountManager.UpdateProfileAsync(_profileModel);
		if (response.Succeeded)
		{
			await _authenticationManager.Logout();
			_navigationManager.NavigateTo("/");
		}
		else
		{
			foreach (var message in response.Messages)
			{
				_snackBar.Add(message, Severity.Error);
			}
		}
	}

	private async Task UploadFiles(InputFileChangeEventArgs e)
	{
		_file = e.File;
		if (_file != null)
		{
			var extension = Path.GetExtension(_file.Name);
			var fileName = $"{UserId}-{Guid.NewGuid()}{extension}";
			var format = "image/png";
			var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
			var buffer = new byte[imageFile.Size];
			await imageFile.OpenReadStream().ReadAsync(buffer);
			var request = new IDM_007 { Data = buffer, FileName = fileName, Extension = extension, UploadType = UploadType.ProfilePicture };
			var result = await _accountManager.UpdateProfilePictureAsync(request, UserId);
			if (result.Succeeded)
			{
				_navigationManager.NavigateTo("/account", true);
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

	private async Task DeleteAsync()
	{
		var parameters = new DialogParameters
			{
				{
					nameof(DeleteConfirmation.ContentText), $"{string.Format("Вы действительно хотите удалить изображение профиля {0}?", _profileModel.Email)}?"
				}
			};
		var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
		var dialog = _dialogService.Show<DeleteConfirmation>("Удаление", parameters, options);
		var dialogResult = await dialog.Result;
		if (!dialogResult.Cancelled)
		{
			var request = new IDM_007 { Data = null, FileName = string.Empty, UploadType = UploadType.ProfilePicture };
			var result = await _accountManager.UpdateProfilePictureAsync(request, UserId);
			if (result.Succeeded)
			{
				await _localStorage.RemoveItemAsync(StorageConstants.Local.UserImageURL);
				ImageDataUrl = string.Empty;
				_navigationManager.NavigateTo("/account", true);
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
}
using MudBlazor;
using Shared.Messages.Master;
using WebUI.Application.Extensions;
using WebUI.Client.Pages.Identity;

namespace WebUI.Client.Pages.Services;

public partial class MasterServices
{
	private bool _isLoaded;
	private List<MasterServiceResponse> _services;
	protected override async Task OnInitializedAsync()
	{
		await LoadDataAsync();
	}

	private async Task LoadDataAsync()
	{
		var state = await _stateProvider.GetAuthenticationStateAsync();
		var result = await _masterManager.GetAllServicesAsync(state.User.GetUserId());
		if (result.Succeeded)
		{
			_services = result.Data.Services;
		}
		else
		{
			foreach (var error in result.Messages)
			{
				_snackBar.Add(error, Severity.Error);
			}
		}
		_isLoaded = true;
	}

	private async Task OpenCreateServiceDialog()
	{
		var parameters = new DialogParameters();
		var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
		var dialog = _dialogService.Show<CreateServiceDialog>("Новая услуга", parameters, options);
		var result = await dialog.Result;
		if (!result.Cancelled)
		{
			await LoadDataAsync();
			await InvokeAsync(() => StateHasChanged());
		}
	}
}
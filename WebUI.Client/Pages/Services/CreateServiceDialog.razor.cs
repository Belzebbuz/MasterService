using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Messages.Master;
using WebUI.Application.Extensions;

namespace WebUI.Client.Pages.Services;

public partial class CreateServiceDialog
{
	private FluentValidationValidator? _fluentValidationValidator;
	private bool Validated => _fluentValidationValidator!.Validate(options => { options.IncludeAllRuleSets(); });
	[CascadingParameter] private MudDialogInstance MudDialog { get; set; }
	private MSM_002 _masterServiceModel = new();

	protected override async Task OnInitializedAsync()
	{
		var state = await _stateProvider.GetAuthenticationStateAsync();
		_masterServiceModel.UserId = state.User.GetUserId();
	}
	private void Cancel()
	{
		MudDialog.Cancel();
	}

	private async Task SubmitAsync()
	{
		var response = await _masterManager.CreateServiceAsync(_masterServiceModel);
		if (response.Succeeded)
		{
			_snackBar.Add("Услуга успешно создана", Severity.Success);
			MudDialog.Close();
		}
		else
		{
			foreach (var message in response.Messages)
			{
				_snackBar.Add(message, Severity.Error);
			}
		}
	}
}
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Messages.Master;

namespace WebUI.Client.Pages.Services;

public partial class ServiceTable
{
	[Parameter] public string Class { get; set; }
	[Parameter] public string UserId { get; set; }
	private List<MasterServiceResponse> _services;
	private bool _isLoaded;
	protected override async Task OnInitializedAsync()
	{
		var result = await _masterManager.GetAllServicesAsync(UserId);
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
}
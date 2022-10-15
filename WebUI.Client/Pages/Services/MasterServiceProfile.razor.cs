using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Shared.Messages.Identity;
using Shared.Messages.Master;
using Shared.Wrapper;
using System;
using System.Threading.Tasks;
using WebUI.Client.Pages.Identity;
using WebUI.Client.Share;
using static MudBlazor.CategoryTypes;

namespace WebUI.Client.Pages.Services;

public partial class MasterServiceProfile
{
	[Parameter] public string Id { get; set; }
	private FluentValidationValidator? _fluentValidationValidator;
	private bool Validated => _fluentValidationValidator!.Validate(options => { options.IncludeAllRuleSets(); });
	private MasterServiceResponse _service;
	private MSM_005 _serviceUpdateModel = new();
	private bool _isLoaded;
	private IBrowserFile _file;
	private int _imageSelectedIndex = 0;
	protected override async Task OnInitializedAsync()
	{
		var response = await _masterManager.GetServiceAsync(Guid.Parse(Id));
		if (response.Succeeded)
		{
			_service = response.Data.MasterService;
			_serviceUpdateModel.ServiceId = _service.ServiceId;
			_serviceUpdateModel.Name = _service.Name;
			_serviceUpdateModel.Description = _service.Description;
			_serviceUpdateModel.NewPriceValue = _service.CurrentPrice;
		}
		else
		{
			foreach (var error in response.Messages)
			{
				_snackBar.Add(error, Severity.Error);
			}
		}
		_isLoaded = true;
	}

	private async Task UploadFile(InputFileChangeEventArgs e)
	{
		_file = e.File;
		if (_file != null)
		{
			var extension = Path.GetExtension(_file.Name);
			var fileName = $"{_service.ServiceId}-{Guid.NewGuid()}{extension}";
			var format = "image/png";
			var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
			var buffer = new byte[imageFile.Size];
			await imageFile.OpenReadStream().ReadAsync(buffer);
			var request = new MSM_003 { Data = buffer, FileName = fileName, Extension = extension, UploadType = UploadType.Product, ServiceId = _service.ServiceId };
			var result = await _masterManager.AddExamplePictureAsync(request);
			await HandleResponseResult(result, "Изображение добавлено!");
		}
	}

	public async Task UpdateMasterServiceAsync()
	{
		var result = await _masterManager.UpdateServiceAsync(_serviceUpdateModel);
		await HandleResponseResult(result, "Изменения применены!");
	}

	private async Task DeleteAsync(int index)
	{
		if (_service.Images.Any())
		{
			var selectedImage = _service.Images[index];
			var result = await _masterManager.DeleteExamplePictureAsync(_service.ServiceId, selectedImage.Id);
			await HandleResponseResult(result, "Изображение удалено!");
		}

	}

	private async Task HandleResponseResult(IResult result, string successMessage)
	{
		if (result.Succeeded)
		{
			_snackBar.Add(successMessage, Severity.Success);
			await OnInitializedAsync();
			await InvokeAsync(() => StateHasChanged());
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
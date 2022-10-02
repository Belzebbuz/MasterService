using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using WebUI.Client;
using WebUI.Client.Shared;
using Blazored.FluentValidation;
using Shared.Messages.Identity;
using System.Security.Claims;
using System.Net;
using MudBlazor;

namespace WebUI.Client.Pages.Authentication
{
	public partial class Login
    {
		private FluentValidationValidator? _fluentValidationValidator;
		private bool Validated => _fluentValidationValidator!.Validate(options => { options.IncludeAllRuleSets(); });
		private IDM_002 _tokenRequest = new();

		protected override async Task OnInitializedAsync()
		{
			var state = await _stateProvider.GetAuthenticationStateAsync();
			if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
			{
				_navigationManager.NavigateTo("/");
			}
		}

		private async Task SubmitAsync()
		{
			var result = await _authenticationManager.Login(_tokenRequest);
			if (result.Succeeded)
			{
				_snackBar.Add($"{_tokenRequest.Email}", Severity.Success);
				_navigationManager.NavigateTo("/", true);
			}
			else
			{
				foreach (var message in result.Messages)
				{
					_snackBar.Add(message, Severity.Error);
				}
			}
		}

		private bool _passwordVisibility;
		private InputType _passwordInput = InputType.Password;
		private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

		void TogglePasswordVisibility()
		{
			if (_passwordVisibility)
			{
				_passwordVisibility = false;
				_passwordInputIcon = Icons.Material.Filled.VisibilityOff;
				_passwordInput = InputType.Password;
			}
			else
			{
				_passwordVisibility = true;
				_passwordInputIcon = Icons.Material.Filled.Visibility;
				_passwordInput = InputType.Text;
			}
		}
	}
}
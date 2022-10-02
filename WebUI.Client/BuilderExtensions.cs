using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using WebUI.Application.Services;
using WebUI.Infrastructure;
using WebUI.Infrastructure.Authentication;

namespace WebUI.Client;

public static class BuilderExtensions
{
	public static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
	{
		builder.Services.AddServices();
		builder.Services.AddAuthorizationCore();
		builder.Services.AddBlazoredLocalStorage();
		builder.Services.AddMudServices(configuration =>
		{
			configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
			configuration.SnackbarConfiguration.HideTransitionDuration = 100;
			configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
			configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
			configuration.SnackbarConfiguration.ShowCloseIcon = false;
		});

		builder.Services.AddScoped<WebAuthStateProvider>();
		builder.Services.AddScoped<AuthenticationStateProvider, WebAuthStateProvider>();
		builder.Services.AddTransient<AuthenticationHeaderHandler>();
		builder.Services.AddScoped(sp =>
			sp.GetRequiredService<IHttpClientFactory>()
			.CreateClient("Love.Nails.Api"))
			.AddHttpClient("Love.Nails.Api", client =>
			{
				client.DefaultRequestHeaders.AcceptLanguage.Clear();
				client.BaseAddress = new Uri("http://localhost:5209");
			})
			.AddHttpMessageHandler<AuthenticationHeaderHandler>();
		return builder;
	}
	
}

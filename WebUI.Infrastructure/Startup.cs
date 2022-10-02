using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebUI.Application.Services;
using WebUI.Application.Services.Managers;
using WebUI.Infrastructure.Managers;

namespace WebUI.Infrastructure;

public static class Startup
{
	public static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddTransient<IAuthenticationManager, AuthenticationManager>();
		services.AddTransient<IUserManager, UserManager>();
		return services;
	}
}

using Infrastructure.Auth;
using Infrastructure.Common;
using Infrastructure.Context;
using Infrastructure.Cors;
using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Startup
{
	public static IServiceCollection AddInfrascructure(this IServiceCollection services, IConfiguration config)
	{
		services
			.AddServices()
			.AddAuth(config)
			.AddPersistence(config)
			.AddScoped<ExceptionMiddleware>()
			.AddScoped<RequestLoggingMiddleware>()
			.AddScoped<ResponseLoggingMiddleware>()
			.AddCorsPolicy();
		return services;
	}

	public static async Task UseInfrastructure(this IApplicationBuilder app)
	{
		app.UseStaticFiles();
		app.UseMiddleware<ExceptionMiddleware>();
		app.UseRouting();
		app.UseCorsPolicy();
		app.UseAuthentication();
		app.UseMiddleware<CurrentUserMiddleware>();
		app.UseAuthorization();
		app.UseMiddleware<RequestLoggingMiddleware>();
		app.UseMiddleware<ResponseLoggingMiddleware>();
		await app.InitDatabaseAsync<ApplicationDbContext>();
	}
}

using Application.Common;
using Domain.Models;
using Infrastructure.Auth.Jwt;
using Infrastructure.Context;
using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Auth;

public static class Startup
{
	internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
	{
		services
			.AddIdentity()
			.AddJwtAuth(config)
			.AddScoped<CurrentUserMiddleware>()
			.AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());
		return services;
	}

	internal static IServiceCollection AddIdentity(this IServiceCollection services) =>
		services
			.AddIdentity<AppUser, IdentityRole>(options =>
			{
				options.Password.RequiredLength = 8;
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = true;
				options.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders()
			.Services;
}

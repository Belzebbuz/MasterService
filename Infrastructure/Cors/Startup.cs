using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cors;

public static class Startup
{
	private const string CorsPolicy = nameof(CorsPolicy);
	internal static IServiceCollection AddCorsPolicy(this IServiceCollection services)
	{
		return services.AddCors(opt =>
	   opt.AddPolicy(CorsPolicy, policy =>
		   policy.AllowAnyHeader()
			   .AllowAnyMethod()
			   .AllowAnyOrigin()));
	}
	internal static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) => app.UseCors(CorsPolicy);
}

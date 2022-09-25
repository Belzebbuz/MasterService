using Application.Persistance;
using Domain.Common;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Infrastructure.Context;

public static class Startup
{
	internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
	{
		var dbSettings = GetDbSettings(config);
		return services
			.Configure<DatabaseSettings>(config.GetSection(nameof(DatabaseSettings)))
			.AddDbContext<ApplicationDbContext>(m => m.UseDatabase(dbSettings.DBProvider!, dbSettings.ConnectionString!))
			.AddRepositories();
	}
	private static IServiceCollection AddRepositories(this IServiceCollection services)
	{
		services.AddScoped(typeof(IRepository<>), typeof(ApplicationDbRepository<>));

		foreach (var aggregateRootType in
				 typeof(IAggregateRoot).Assembly.GetExportedTypes()
					 .Where(t => typeof(IAggregateRoot).IsAssignableFrom(t) && t.IsClass)
					 .ToList())
		{
			services.AddScoped(typeof(IReadRepository<>).MakeGenericType(aggregateRootType), sp =>
				sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)));

		}
		return services;
	}

	public static async Task InitDatabaseAsync<T>(this IApplicationBuilder app) where T: DbContext
	{
		using var scope = app.ApplicationServices.CreateScope();
		var context = scope.ServiceProvider.GetService<T>();
		await context.Database.MigrateAsync();
	}
	private static DatabaseSettings GetDbSettings(IConfiguration config)
	{
		var databaseSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
		string? rootConnectionString = databaseSettings.ConnectionString;
		if (string.IsNullOrEmpty(rootConnectionString))
		{
			throw new InvalidOperationException("DB ConnectionString is not configured.");
		}

		string? dbProvider = databaseSettings.DBProvider;
		if (string.IsNullOrEmpty(dbProvider))
		{
			throw new InvalidOperationException("DB Provider is not configured.");
		}
		return databaseSettings;
	}

	internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
	{
		switch (dbProvider.ToLowerInvariant())
		{
			case DbProviderKeys.Npgsql:
				AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
				return builder.UseNpgsql(connectionString, e =>
					e.MigrationsAssembly("Migrator.PostgreSQL"));

			case DbProviderKeys.SqlServer:
				return builder.UseSqlServer(connectionString, e =>
					e.MigrationsAssembly("Migrator.MSSQL"));

			case DbProviderKeys.Sqlite:
				return builder.UseSqlite(connectionString, e =>
					e.MigrationsAssembly("Migrator.Sqlite"));

			default:
				throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
		}
	}
}

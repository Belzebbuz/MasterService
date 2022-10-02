using Application;
using Infrastructure;
using Infrastructure.Common;
using Serilog;

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
	var builder = WebApplication.CreateBuilder(args);
	builder.Host.UseSerilog((_, config) =>
	{
		config.WriteTo.Console()
		.ReadFrom.Configuration(builder.Configuration);
	});
	builder.Services.AddControllers();
	builder.Services.AddSwaggerGen();
	builder.Services.AddInfrascructure(builder.Configuration);
	builder.Services.AddApplication();


	var app = builder.Build();

	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}
	await app.UseInfrastructure();
	app.MapControllers();

	app.Run();

}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
	StaticLogger.EnsureInitialized();
	Log.Fatal(ex, "Unhandled exception");
}
finally
{
	StaticLogger.EnsureInitialized();
	Log.Information("Server Shutting down...");
	Log.CloseAndFlush();
}

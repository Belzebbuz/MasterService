﻿using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using System.Net.Http;
using System.Text;

namespace Infrastructure.Middlewares;

public class RequestLoggingMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		LogContext.PushProperty("RequestTimeUTC", DateTime.UtcNow);
		string requestBody = string.Empty;
		if (context.Request.Path.ToString().Contains("tokens"))
		{
			requestBody = "[Redacted] Contains Sensitive Information.";
		}
		else
		{
			var request = context.Request;

			if (!string.IsNullOrEmpty(request.ContentType)
				&& request.ContentType.StartsWith("application/json"))
			{
				request.EnableBuffering();
				using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 4096, true);
				requestBody = await reader.ReadToEndAsync();

				request.Body.Position = 0;
			}
		}

		LogContext.PushProperty("RequestBody", requestBody);
		Log.ForContext("RequestHeaders", context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
		.ForContext("RequestBody", requestBody)
			.Information("HTTP {RequestMethod} Request sent to {RequestPath}", context.Request.Method, context.Request.Path);
		await next(context);
	}
}

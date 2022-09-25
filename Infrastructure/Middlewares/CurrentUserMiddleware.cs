﻿using Application.Common.Contracts;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Middlewares;

public class CurrentUserMiddleware : IMiddleware
{
	private readonly ICurrentUserInitializer _currentUserInitializer;

	public CurrentUserMiddleware(ICurrentUserInitializer currentUserInitializer) =>
		_currentUserInitializer = currentUserInitializer;

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		_currentUserInitializer.SetCurrentUser(context.User);

		await next(context);
	}
}

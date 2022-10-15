using Application.Common;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Shared.Exceptions.Common;
using Shared.Wrapper;
using System.Net;

namespace Infrastructure.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
	private readonly ICurrentUser _currentUser;
	private readonly ISerializerService _jsonSerializer;

	public ExceptionMiddleware(ICurrentUser currentUser, ISerializerService jsonSerializer)
	{
		_currentUser = currentUser;
		_jsonSerializer = jsonSerializer;
	}
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (Exception exception)
		{
			string email = _currentUser.GetUserEmail() is string userEmail ? userEmail : "Anonymous";
			var userId = _currentUser.GetUserId();
			string errorId = Guid.NewGuid().ToString();
			var errorResult = new ErrorResult
			{
				Source = exception.TargetSite?.DeclaringType?.FullName,
				Exception = exception.Message.Trim(),
				ErrorId = errorId
			};
			errorResult.Messages!.Add(exception.Message);
			var response = context.Response;
			response.ContentType = "application/json";
			if (exception is not CustomException && exception.InnerException != null)
			{
				while (exception.InnerException != null)
				{
					exception = exception.InnerException;
				}
			}
			switch (exception)
			{
				case CustomException e:
					response.StatusCode = errorResult.StatusCode = (int)e.StatusCode;
					if (e.ErrorMessages is not null)
					{
						errorResult.Messages = e.ErrorMessages;
					}

					break;

				case KeyNotFoundException:
					response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.NotFound;
					break;

				default:
					response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
					break;
			}
			var stringResult = _jsonSerializer.Serialize(errorResult);
			LogContext.PushProperty("Error", stringResult);
			await response.WriteAsync(_jsonSerializer.Serialize(Result.Fail(stringResult)));
		}
	}
}

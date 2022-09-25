using Shared.Exceptions.Common;
using System.Net;

namespace Shared.Exceptions.Auth;

public class UnauthorizedException : CustomException
{
	public UnauthorizedException(string message)
	   : base(message, null, HttpStatusCode.Unauthorized)
	{
	}
}

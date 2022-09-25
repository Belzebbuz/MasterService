using Shared.Exceptions.Common;
using System.Net;

namespace Shared.Exceptions.Auth;

public class ForbiddenException : CustomException
{
	public ForbiddenException(string message)
		: base(message, null, HttpStatusCode.Forbidden)
	{
	}
}

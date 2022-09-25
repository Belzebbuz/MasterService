using System.Net;

namespace Shared.Exceptions.Common;

public class InternalServerException : CustomException
{
	public InternalServerException(string message, List<string>? errors = default)
		: base(message, errors, HttpStatusCode.InternalServerError)
	{
	}
}

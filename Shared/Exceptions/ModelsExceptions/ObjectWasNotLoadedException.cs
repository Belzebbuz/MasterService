using Shared.Exceptions.Common;
using System.Net;

namespace Shared.Exceptions.ModelsExceptions;

public class ObjectWasNotLoadedException : CustomException
{
    public ObjectWasNotLoadedException(string requiredObjectName, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
        : base($"Required object: {requiredObjectName} was not loaded", null, statusCode)
    {
    }
}

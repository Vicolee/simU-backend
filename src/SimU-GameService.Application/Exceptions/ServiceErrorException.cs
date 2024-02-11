using System.Net;

namespace SimU_GameService.Application.Common.Exceptions;

public class ServiceErrorException : Exception
{
    public ServiceErrorException(HttpStatusCode statusCode, string content)
        : base($"Service error occurred. Status code: {statusCode}. Content: {content}")
    {
    }
}

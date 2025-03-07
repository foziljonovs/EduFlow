using System.Net;

namespace EduFlow.BLL.Common.Exceptions;

public class StatusCodeException(HttpStatusCode code, string message) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = code;
}
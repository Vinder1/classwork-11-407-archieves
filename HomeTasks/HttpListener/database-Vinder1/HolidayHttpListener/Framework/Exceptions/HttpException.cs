namespace HolidayHttpListener.Framework.Exceptions;

public class HttpException(StatusCode statusCode)
: Exception(HttpCodeToMessage.GetFrom(statusCode))
{
    public StatusCode StatusCode { get; init; } = statusCode;
}

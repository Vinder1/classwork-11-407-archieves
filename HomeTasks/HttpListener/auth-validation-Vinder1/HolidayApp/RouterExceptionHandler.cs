using Emerald;
using Emerald.Exceptions;

namespace HolidayApp;

public class RouterExceptionHandler : IExceptionHandler
{
    public async Task OnException(Emerald.AppContext appContext, Exception exception)
    {
        switch (exception)
        {
            case HttpException httpException:
                await OnHttpException(appContext, httpException);
                break;
            default:
                await appContext.WritePlainText("Error 500: internal error");
                break;
        }
    }

    private async Task OnHttpException(Emerald.AppContext appContext, HttpException exception)
    {
        appContext.Response.StatusCode = exception.StatusCode.ToInt();
        appContext.Response.ContentType = "text/plain; charset=utf-8";
        await appContext.WritePlainText($"Ошибка {exception.StatusCode.ToInt()}: {HttpCodeToMessage.GetFrom(exception.StatusCode)}");
    }
}

using HolidayHttpListener.ExternalApi;
using HolidayHttpListener.Framework;
using HolidayHttpListener.Framework.Controller;

namespace HolidayHttpListener.Controllers;

[Controller]
public class HolidayApiController
{
    private readonly HolidayApiClient client = new();

    [Controller.Get("api/holiday")]
    public async Task Holiday(Framework.AppContext httpContext)
    {
        var data = await client.RequestAsString();
        await httpContext.WritePlainText(data);
    }
}
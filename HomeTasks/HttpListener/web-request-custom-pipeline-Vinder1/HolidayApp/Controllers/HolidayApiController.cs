using Emerald;
using Emerald.Controller;
using Emerald.Exceptions;
using HolidayApp.ExternalApi;

namespace HolidayApp.Controllers;

[Controller]
public class HolidayApiController
{
    private readonly HolidayApiClient client = new();

    [Controller.Get("api/holiday")]
    public async Task Holiday(Emerald.AppContext context)
    {
        var data = await client.RequestAsString();
        context.SetResponseStatusCode(StatusCode.OK);
        await context.WritePlainText(data);
    }
}
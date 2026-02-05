using Emerald;
using Emerald.Controller;
using Emerald.Exceptions;

namespace HolidayApp.Controllers;

[Controller]
public class AdminController
{
    [Controller.Get("admin")]
    public async Task AdminPage(Emerald.AppContext context)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.SendFile("admin.html");
    }
}
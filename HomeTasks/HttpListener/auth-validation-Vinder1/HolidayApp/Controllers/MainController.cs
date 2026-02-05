using Emerald;
using Emerald.Controller;
using Emerald.Exceptions;

namespace HolidayApp.Controllers;

[Controller]
public class MainController
{
    [Controller.Get("home")]
    public async Task Main(Emerald.AppContext context)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.SendFile("main.html");
    }

    [Controller.Get("")]
    public async Task RedirectToHome(Emerald.AppContext context)
    {
        context.Response.Redirect("/home");
        await Task.CompletedTask;
    }
}

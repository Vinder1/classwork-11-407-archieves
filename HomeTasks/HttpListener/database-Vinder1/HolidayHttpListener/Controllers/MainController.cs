using System.Net;
using HolidayHttpListener.Framework;
using HolidayHttpListener.Framework.Controller;

namespace HolidayHttpListener.Controllers;

[Controller]
public class MainController
{
    [Controller.Get("home")]
    public async Task Main(Framework.AppContext context)
    {
        await context.SendFile("main.html");
    }

    [Controller.Get("")]
    public async Task RedirectToHome(Framework.AppContext context)
    {
        context.Response.Redirect("/home");
        await Task.CompletedTask;
    }
}

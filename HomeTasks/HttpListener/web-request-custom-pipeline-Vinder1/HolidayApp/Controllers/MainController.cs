using Emerald.Controller;

namespace HolidayApp.Controllers;

[Controller]
public class MainController
{
    [Controller.Get("home")]
    public async Task Main(Emerald.AppContext context)
    {
        await View.RenderHtmlFile(context, "main");
    }

    [Controller.Get("")]
    public async Task RedirectToHome(Emerald.AppContext context)
    {
        context.Response.Redirect("/home");
        await Task.CompletedTask;
    }
}

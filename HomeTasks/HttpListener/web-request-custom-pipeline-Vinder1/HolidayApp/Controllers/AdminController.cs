using Emerald.Controller;

namespace HolidayApp.Controllers;

[Controller]
public class AdminController
{
    [Controller.Get("admin")]
    [Authorized(role: "admin")]
    public async Task AdminPage(Emerald.AppContext context)
    {
        await View.RenderHtmlFile(context, "admin");
    }
}
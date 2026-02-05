using Emerald.Controller;

namespace HolidayApp.Controllers;

[Controller]
public class HelpController
{

    [Controller.Get(Path: "about")]
    public async Task About(Emerald.AppContext context)
    {
        await View.RenderHtmlFile(context, "about");
    }

    [Controller.Get(Path: "contact")]
    public async Task Contact(Emerald.AppContext context)
    {
        await View.RenderHtmlFile(context, "contact");
    }
}

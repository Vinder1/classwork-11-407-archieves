using Emerald.Controller;

namespace HolidayApp.Controllers;

[Controller]
public class AmberController
{
    [Controller.Get("amber")]
    public async Task Main(Emerald.AppContext context)
    {
        await View.RenderHtmlFile(context, "amber");
    }
}

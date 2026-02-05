using Emerald;
using Emerald.Controller;
using Emerald.Exceptions;

namespace HolidayApp.Controllers;

[Controller]
public class HelpController
{

    [Controller.Get(Path: "about")]
    public async Task About(Emerald.AppContext context)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.SendFile("about.html");
    }

    [Controller.Get(Path: "contact")]
    public async Task Contact(Emerald.AppContext context)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.SendFile("contact.html");
    }
}

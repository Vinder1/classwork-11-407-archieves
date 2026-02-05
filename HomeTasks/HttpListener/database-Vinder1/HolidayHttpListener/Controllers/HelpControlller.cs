using System.Net;
using System.Text;
using HolidayHttpListener.ExternalApi;
using HolidayHttpListener.Framework;
using HolidayHttpListener.Framework.Controller;

namespace HolidayHttpListener.Controllers;

[Controller]
public class HelpController
{

    [Controller.Get(Path: "about")]
    public async Task About(Framework.AppContext context)
    {
        await context.SendFile("about.html");
    }

    [Controller.Get(Path: "contact")]
    public async Task Contact(Framework.AppContext context)
    {
        await context.SendFile("contact.html");
    }
}

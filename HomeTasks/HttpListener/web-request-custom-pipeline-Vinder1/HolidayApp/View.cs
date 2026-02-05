using Emerald;
using Emerald.Exceptions;

namespace HolidayApp;

public static class View
{
    public static async Task RenderHtmlFile(Emerald.AppContext context, string name)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.SendFile(Path.Combine("html", $"{name}.html"));
    }
}

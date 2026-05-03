var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapGet("/", () => "Hi! Go to /home");
app.MapGet("/home", async context => await SendHtmlFileAsync(context, "main"));
app.MapGet("/about", async context => await SendHtmlFileAsync(context, "about"));
app.MapGet("/contact", async context => await SendHtmlFileAsync(context, "contact"));

static async Task SendHtmlFileAsync(HttpContext context, string fileName)
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync($"wwwroot/{fileName}.html");
}

app.Map("/holiday", apiApp =>
{
    apiApp.Use(async (context, next) =>
    {
        try
        {
            await next();
        }
        catch (Exception e)
        {
            Console.WriteLine($"API упал! {e.Message}");
        }
    });

    apiApp.Run(async context =>
    {
        var data = await AspNetApp.HolidayApiClient.RequestAsString();
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(data);
    });
});

app.Run();

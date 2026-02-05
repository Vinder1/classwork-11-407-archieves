using System.Net;
using System.Text;

namespace HolidayHttpListener;

public class RequestHandler(string Url)
{
    public void Invoke(HttpListenerContext context)
    {
        var response = context.Response;
        var request = context.Request;
        var path = request.Url?.LocalPath;

        _ = Task.Run(async () =>
        {
            await Respond(path, response);            
            response.OutputStream.Close();
            response.Close();
        });
    }

    private async Task Respond(string? path, HttpListenerResponse response)
    {
        switch (path)
        {
            case "/":
                response.Redirect(Url + "home");
                break;

            case "/home":
                await LocalFileManager.SendPageFile(response, "main.html");
                break;

            case "/about":
                await LocalFileManager.SendPageFile(response, "about.html");
                break;

            case "/contact":
                await LocalFileManager.SendPageFile(response, "contact.html");
                break;

            case "/holiday":
                var data = await HolidayApiClient.RequestAsString();
                await response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(data));
                break;

            case string filePath when LocalFileManager.FileExists(filePath):
                await LocalFileManager.SendPageFile(response, filePath);
                break;

            default:
                response.StatusCode = 404;
                response.ContentType = "text/plain; charset=utf-8";
                await response.OutputStream.WriteAsync("Не туда зашёл"u8.ToArray());
                break;
        }
    }
}

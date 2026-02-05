using System.Text;
using System.Text.Json;

namespace HolidayHttpListener.Framework;

public static class AppContextExtensions
{
    public static async Task SendFile(this AppContext appContext, string path)
    {
        await appContext.FileProvider.SendFile(appContext.Response, path);
    }

    public static async Task WritePlainText(this AppContext appContext, string text)
    {
        appContext.Response.ContentType = "text/plain; charset=utf-8";
        await appContext.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(text));
    }

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public static async Task<T?> ReadJson<T>(this AppContext appContext, CancellationToken ct)
    {
        using var stream = new StreamReader(appContext.Request.InputStream, appContext.Request.ContentEncoding);
        var body = await stream.ReadToEndAsync(ct);
        return string.IsNullOrWhiteSpace(body) ? default : JsonSerializer.Deserialize<T>(body, JsonOpts);
    }

    public static async Task WriteJson(this AppContext appContext, object obj)
    {
        var json = JsonSerializer.Serialize(obj, JsonOpts);
        var response = appContext.Response;
        response.StatusCode = 200;
        response.ContentType = "application/json; charset=utf-8";
        await response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(json));
    }

    public static HttpMethod GetHttpMethod(this AppContext appContext)
    {
        return appContext.Request.HttpMethod switch
        {
            "GET" => HttpMethod.GET,
            "POST" => HttpMethod.POST,
            "PUT" => HttpMethod.PUT,
            "DELETE" => HttpMethod.DELETE,
            "PATCH" => HttpMethod.PATCH,
            _ => throw new UnsupportedHttpMethodException(appContext.Request.HttpMethod),
        };
    }

    public class UnsupportedHttpMethodException(string method) : Exception($"{method} is not supported") {}
}

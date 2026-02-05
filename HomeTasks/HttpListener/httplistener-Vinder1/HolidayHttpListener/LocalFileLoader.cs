using System.Net;

namespace HolidayHttpListener;

public class LocalFileManager
{
    public static bool FileExists(string path)
        => FilesURNs.Contains(path);

    private static string[] FilesURNs { get; } = FindAllPublicFiles();

    private static string[] FindAllPublicFiles()
    {
        return Directory.GetFiles("public/")
            .Union(Directory.GetFiles("public/image/"))
            .Select(s => s[s.IndexOf('/')..]) //skip "public"
            .ToArray();
    }

    public static async Task SendPageFile(HttpListenerResponse response, string path)
    {
        var file = await File.ReadAllBytesAsync($"public/{path}");
        response.ContentType = $"text/{Path.GetExtension(path)[1..]}; charset=utf-8";
        response.ContentLength64 = file.Length;
        await response.OutputStream.WriteAsync(file);
    }
}

using System.ComponentModel.Design.Serialization;
using System.Net;

namespace HolidayHttpListener.Framework;

public class LocalFileProvider(string root)
{
    public bool FileExists(string path)
        => FilesURNs.Contains(PathWithoutSlash(path));

    private string[] FilesURNs { get; } = FindAllPublicFiles(root);
    private string Root { get; } = root;

    private static string[] FindAllPublicFiles(string root)
    {
        var allFiles = UniteWithSubDirectories(root).SelectMany(Directory.GetFiles);
        allFiles = allFiles.Select(path => path.Replace("\\", "/"));
        for (int i = 0; i < root.Split('/').Length; i++)
        {
            allFiles = allFiles.Select(s => s[(s.IndexOf('/') + 1)..]);
        }

        var arr = allFiles.ToArray();
        return arr;
    }

    /// <summary>
    /// Get the Enumerable object, containing this directory and ALL the directories inside it;
    /// It works recursively
    /// </summary>
    private static IEnumerable<string> UniteWithSubDirectories(string root)
        => Directory.GetDirectories(root).SelectMany(UniteWithSubDirectories).Append(root);

    private string PathWithoutSlash(string path)
    {
        if (path.StartsWith("/"))
            return path[1..];
        return path;
    }

    public async Task SendFile(HttpListenerResponse response, string path)
    {
        path = PathWithoutSlash(path);
        var file = await File.ReadAllBytesAsync($"{Root}/{path}");
        response.ContentType = $"{GetContentTypeForFile(path)}; charset=utf-8";
        response.ContentLength64 += file.Length;
        await response.OutputStream.WriteAsync(file);
    }

    private string GetContentTypeForFile(string path)
    {
        var e = Path.GetExtension(path)[1..]; 
        return e switch
        {
            "html" => "text/html",
            "js" => "application/javascript",
            "json" => "application/json",
            "css" => "text/css",
            "png" => "image/png",
            "ico" => "image/x-icon",
            _ => "text/plain"
        };
    }
}

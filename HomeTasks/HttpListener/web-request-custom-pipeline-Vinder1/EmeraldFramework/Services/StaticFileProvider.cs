using System.Net;

namespace Emerald;

/// <summary>
/// Standard default service that loads files 
/// </summary>
/// <param name="root"></param>
public class StaticFileProvider(string root)
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

    public async Task SendFile(AppContext context, string path)
    {
        path = PathWithoutSlash(path);

        context.Response.ContentType = $"{GetContentTypeForFile(path)}; charset=utf-8";
        context.Response.StatusCode = 200;
        
        if (HasCached(context, path))
        {
            context.Response.StatusCode = 304;
            return;
        }

        await WriteFileToResponse(context, path);
    }

    private bool HasCached(AppContext context, string path)
    {
        var lastModifiedUtc = File.GetLastWriteTimeUtc(path);
        var etag = $"{Path.GetFileName(path)}-{lastModifiedUtc.Ticks}";

        context.Response.AddHeader("Cache-Control", "public, max-age=3600");
        context.Response.AddHeader("Last-Modified", lastModifiedUtc.ToString("R"));
        context.Response.AddHeader("ETag", etag);

        var clientEtag = context.Request.Headers["If-None-Match"];
        if (clientEtag == etag)
            return true;

        var clientModifiedSince = context.Request.Headers["If-Modified-Since"];
        return DateTime.TryParse(clientModifiedSince, out var since) && lastModifiedUtc <= since;
    }

    private async Task WriteFileToResponse(AppContext context, string path)
    {
        var file = await File.ReadAllBytesAsync($"{Root}/{path}");
        context.Response.ContentLength64 += file.Length;
        await context.Response.OutputStream.WriteAsync(file);
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
            "jpg" => "image/jpg",
            "png" => "image/png",
            "ico" => "image/x-icon",
            _ => "text/plain"
        };
    }
}

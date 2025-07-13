using System.IO.Compression;

namespace MediaContentManager;

public class ZipMediaListImporter
{
    
    /// <summary>
    /// Zip file contains files with lists of items
    /// </summary>
    public List<IMediaItem> Import(string zipPath)
    {
        var list = new List<IMediaItem>();
        var factory = new MediaSerializerFactory<List<IMediaItem>>();
        
        using var zipArchive = ZipFile.OpenRead(zipPath);
        foreach (var file in zipArchive.Entries)
        {
            using var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            list.AddRange(factory.GetMediaSerializer(file.FullName).Deserialize(stream));
        }
        return list;
    }
    
    
    /// <summary>
    /// Zip file contains files with only single items
    /// </summary>
    public List<IMediaItem> ImportSingle(string zipPath)
    {
        var list = new List<IMediaItem>();
        var factory = new MediaSerializerFactory<IMediaItem>();
        
        using var zipArchive = ZipFile.OpenRead(zipPath);
        foreach (var file in zipArchive.Entries)
        {
            using var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            list.Add(factory.GetMediaSerializer(file.FullName).Deserialize(stream));
        }
        return list;
    }
}
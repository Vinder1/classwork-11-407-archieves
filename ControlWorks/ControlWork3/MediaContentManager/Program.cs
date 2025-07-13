using System.Reflection;

namespace MediaContentManager;

class Program
{
    static void Main(string[] args)
    {
        var app = new MediaApp();
        var path = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!}/Files/";
        Directory.CreateDirectory(path);
        var filePath = $"{path}Azbuka.json";
        File.Create(filePath).Close();

        var serializer = new JsonMediaSerializer<IMediaItem>();
        using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            serializer.Serialize(fs, new EBook()
            {
                Duration = new TimeSpan(0,0,0),
                Metadata = [],
                PageCount = 156,
                Title = "азбука для чайников",
                Path = filePath,
                Tags = []
            });
        }
        
        app.PlayItem(filePath);
    }
}
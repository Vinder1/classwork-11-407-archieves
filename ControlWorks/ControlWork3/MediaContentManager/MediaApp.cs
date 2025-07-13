namespace MediaContentManager;

public class MediaApp
{
    public Repository<IMediaItem>? MediaLibrary { get; private set; }
    public Playlist Playlist { get; private set; }
    public MediaPlayer Player { get; private set; } = new MediaPlayer();
    private IMediaSerializerFactory<List<IMediaItem>> libraryFactory = new MediaSerializerFactory<List<IMediaItem>>();
    private IMediaSerializerFactory<IMediaItem> singleFactory = new MediaSerializerFactory<IMediaItem>();
    private IMediaSerializerFactory<Playlist> playlistFactory = new MediaSerializerFactory<Playlist>();
    
    public void PlayItem(string path)
    {
        if (MediaLibrary is null)
        {
            CreateMediaLibrary();
        }

        var suchItems = MediaLibrary!.Find(item => item.Path == path).ToArray();
        IMediaItem item;
        if (suchItems.Length == 0)
        {
            item = LoadSingleFile(path);
            MediaLibrary.Add(item);
        }
        else
        {
            item = suchItems.First();
        }

        Player.SetMediaItem(item);
        Player.Play();
        history.Add(item);
    }

    public void PlayPlaylist(string path)
    {
        Playlist = LoadPlaylist(path);
        foreach (var item in Playlist.Items)
        {
            Player.SetMediaItem(item);
            Player.Play();
            // Await while not ended or smth
        }
    }

    private IMediaItem LoadSingleFile(string path)
    {
        var serializer = singleFactory.GetMediaSerializer(path);
        using Stream stream = File.OpenRead(path);
        return serializer.Deserialize(stream);
    }
    private void LoadMediaLibrary(string path)
    {
        if (Player.MediaItem != null)
        {
            Player.SetMediaItem(null);
        }
        
        var serializer = libraryFactory.GetMediaSerializer(path);
        using Stream stream = File.OpenRead(path);
        CreateMediaLibrary();
        var items = serializer.Deserialize(stream);
        foreach (var item in items)
        {
            MediaLibrary!.Add(item);
        }
    }

    private Playlist LoadPlaylist(string path)
    {
        var serializer = playlistFactory.GetMediaSerializer(path);
        using Stream stream = File.OpenRead(path);
        return serializer.Deserialize(stream);
    }

    private void CreateMediaLibrary()
    {
        MediaLibrary = new Repository<IMediaItem>();
    }

    public void AddToMediaLibrary(IMediaItem mediaItem)
    {
        if (MediaLibrary is null)
        {
            CreateMediaLibrary();
        }
        
        MediaLibrary!.Add(mediaItem);
    }
    
    public void ImportFromZip(string zipPath)
    {
        if (MediaLibrary is null)
        {
            CreateMediaLibrary();
        }

        var zipLoader = new ZipMediaListImporter();
        var mediaItems = zipLoader.Import(zipPath);
        foreach (var item in mediaItems)
        {
            MediaLibrary!.Add(item);
        }
    }

    public void SaveMediaLibrary(string path)
    {
        //
    }


    private List<IMediaItem> history = [];
    public List<IMediaItem> ShowHistory()
    {   
        return history.ToList(); // return copy of it
    }
}
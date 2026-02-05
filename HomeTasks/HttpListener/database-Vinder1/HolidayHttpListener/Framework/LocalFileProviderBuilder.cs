namespace HolidayHttpListener.Framework;

public class LocalFileProviderBuilder
{
    public void SetDirectory(string path)
    {
        _directory = path;
    }

    public LocalFileProvider Build()
        => new (_directory);

    private string _directory = "";
}

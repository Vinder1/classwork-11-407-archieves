using HolidayHttpListener.Framework.Exceptions;

namespace HolidayHttpListener.Framework;

public partial class HttpApp
{
    /// <summary>
    /// Provides a path to the directory where static files are located.
    /// Files are loaded recursively from every subdirectory.
    /// Currently, only single directory with files is supported.
    /// </summary>
    /// <param name="path">Relative path to directory</param>
    public void UseStaticFiles(string path)
    {
        handler.LocalFileProviderBuilder.SetDirectory(path);
    }

    /// <summary>
    /// Use <see cref="TExceptionHandler"> as Exception Handler
    /// </summary>
    public void UseExceptionHandler<TExceptionHandler>()
    {
        handler.ExceptionHandler = Activator.CreateInstance<TExceptionHandler>() as IExceptionHandler;
    }
}

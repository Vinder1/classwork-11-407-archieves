namespace Emerald.Exceptions
{
    public interface IExceptionHandler
    {
        public Task OnException(AppContext appContext, Exception exception);
    }
}
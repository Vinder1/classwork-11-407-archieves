namespace HolidayHttpListener.Framework.Exceptions
{
    public interface IExceptionHandler
    {
        public Task OnException(AppContext appContext, Exception exception);
    }
}
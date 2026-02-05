namespace Emerald.Services.Logging
{
    public interface ILogger
    {
        public void StartRequestProcessing(AppContext context);
        public void Write(string message);
    }
}
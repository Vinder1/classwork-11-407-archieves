namespace Emerald.Middleware
{
    public interface IMiddleware
    {
        public IMiddleware? Next { set; get; }
        public Task InvokeAsync(AppContext context);
    }
}
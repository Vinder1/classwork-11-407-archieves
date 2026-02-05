namespace Emerald.Middleware
{
    public interface IMiddleware
    {
        public IMiddleware? Next { set; } //lol, no get
        public Task InvokeAsync(AppContext context);
    }
}
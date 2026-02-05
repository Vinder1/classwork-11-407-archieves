using ControlWorkApp;
using Emerald;
using System.Reflection;
using ControlWorkApp.Middleware;

var app = new HttpApp("http://localhost:5000/");

app.AddServiceConfigurator("config.json");
//app.UseStaticFiles("public");

app.AddControllersFromAssembly(Assembly.GetExecutingAssembly());

app.UseExceptionHandler<RouterExceptionHandler>();

app.AddMiddleware<StopwatchMiddleware>();

app.Build();

var appTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    appTokenSource.Cancel();
};

await app.RunAsync(appTokenSource.Token);
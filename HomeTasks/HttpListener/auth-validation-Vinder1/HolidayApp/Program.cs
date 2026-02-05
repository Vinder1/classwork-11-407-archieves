using HolidayApp;
using Emerald;
using System.Reflection;

var app = new HttpApp("http://localhost:5000/");

app.AddServiceConfigurator("config.json");
app.UseStaticFiles("public");

app.AddControllersFromAssembly(Assembly.GetExecutingAssembly());

app.UseExceptionHandler<RouterExceptionHandler>();

app.Build();

var appTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    appTokenSource.Cancel();
};

await app.RunAsync(appTokenSource.Token);
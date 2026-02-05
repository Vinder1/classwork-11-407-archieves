using HolidayApp;
using Emerald;
using System.Reflection;
using HolidayApp.Middleware;
using Emerald.Services.Logging;
using HolidayApp.Validation;
using HolidayApp.Dto;

var app = new HttpApp("http://localhost:5000/");

app.AddServiceConfigurator("config.json");
app.UseStaticFiles("public");

app.AddControllersFromAssembly(Assembly.GetExecutingAssembly());

app.UseExceptionHandler<RouterExceptionHandler>();
app.UseLogger<ConsoleLogger>();

app.AddValidator<SignUpDto, UserCreateValidator>();
app.AddValidator<UpdateUserDto, UserUpdateValidator>();

app.AddMiddleware<StopwatchMiddleware>();
app.AddStaticFilesMiddleware();
app.AddEndPointFindMiddleware();
app.AddMiddleware<AuthenticationMiddleware>();
app.AddMiddleware<AuthorizationMiddleware>();
app.AddMiddleware<JsonBodyParserMiddleware>();
app.AddMiddleware<ValidationMiddleware>();

app.Build();

var appTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    appTokenSource.Cancel();
};

await app.RunAsync(appTokenSource.Token);
using HolidayHttpListener;
using HolidayHttpListener.Framework;

var app = new HttpApp("http://localhost:5000/");

app.UseStaticFiles("public");
app.UseExceptionHandler<RouterExceptionHandler>();

await app.RunAsync();
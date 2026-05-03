using AspNetApp.Endpoints;
using AspNetApp.PipelineConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAutoMapper();

builder.AddRepositories();
builder.AddServices();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/world", () => Results.File("world.html", "text/html"));

app.MapGalaxyEndpoints();
app.MapStarsEndpoints();
app.MapPlayerEndpoints();
app.MapSpaceshipEndpoints();
app.MapStarVisitEndpoints();

app.Run();

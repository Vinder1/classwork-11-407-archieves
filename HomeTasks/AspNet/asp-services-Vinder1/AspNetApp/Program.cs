using AspNetApp;
using AspNetApp.IRepository;
using AspNetApp.IServices;
using AspNetApp.Repository;
using AspNetApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPlayerRepository>(new JsonPlayerRepository("data/players.json"));
builder.Services.AddSingleton<ISpaceshipRepository>(new JsonSpaceshipRepository("data/spaceships.json"));
builder.Services.AddSingleton<IGalaxyRepository>(new JsonGalaxyRepository("data/galaxy.json"));
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<ISpaceshipService, SpaceshipService>();
builder.Services.AddScoped<IGalaxyService, GalaxyService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/world", () => Results.File("world.html", "text/html"));

// app.MapGet("/stars", () =>
// {
//     return GalaxyGenerator.Generate(100, 0.2, 5);
// });

app.MapGalaxyEndpoints();
app.MapStarsEndpoints();
app.MapPlayerEndpoints();
app.MapSpaceshipEndpoints();

app.Run();

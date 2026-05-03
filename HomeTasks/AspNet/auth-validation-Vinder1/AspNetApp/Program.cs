using AspNetApp.Endpoints;
using AspNetApp.PipelineConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAutoMapper();
builder.ConfigureAuthorization();
builder.ConfigureSwagger();

builder.AddOptions();
builder.AddRepositories();
builder.AddServices();
builder.AddValidation();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapGet("/world", () => Results.File("world.html", "text/html"));

app.MapAuthEndpoints();
app.MapGalaxyEndpoints();
app.MapStarsEndpoints();
app.MapPlayerEndpoints();
app.MapSpaceshipEndpoints();
app.MapStarVisitEndpoints();


app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        options.RoutePrefix = "swagger";
    });
}

app.Run();

using AspNetApp.PipelineConfiguration;
using Carter;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAutoMapper();
builder.ConfigureAuthorization();
builder.ConfigureSwagger();
builder.ConfigureLogger();
builder.ConfigureHttpLogging();

builder.AddOptions();
builder.AddRepositories();
builder.AddServices();
builder.AddValidation();

builder.Services.AddCarter();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.UseHttpMetrics();
app.MapMetrics().AllowAnonymous();

// app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCorrelationIdGenerator();
app.UseCustomSerilogAndHttpLogging();

app.UseAuthentication();  
app.UseAuthorization();

app.MapGet("/world", () => Results.File("world.html", "text/html"));

app.MapCarter();
app.MapControllers();
// app.MapAuthEndpoints();
// app.MapGalaxyEndpoints();
// app.MapStarsEndpoints();
// app.MapPlayerEndpoints();
// app.MapSpaceshipEndpoints();
// app.MapStarVisitEndpoints();

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

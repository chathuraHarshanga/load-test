using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:9090");
var app = builder.Build();

app.MapGet("/greet", () =>
{
    var name = Environment.GetEnvironmentVariable("NAME");

    if (string.IsNullOrEmpty(name))
    {
        return Results.Ok("Hello, NAME environment variable is not set");
    }

    return Results.Ok($"Hello, {name}!");
});

app.Run();

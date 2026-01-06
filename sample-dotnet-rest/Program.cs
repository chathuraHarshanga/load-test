using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:9090");
var app = builder.Build();

// GET /greet/{name}
app.MapGet("/greet/{name}", (string name) => $"Hello, {name}!");

// Optional health check
app.MapGet("/", () => "OK");

app.Run();

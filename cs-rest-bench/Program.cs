using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:9090");
var app = builder.Build();

// GET /benchmark?arraySize=200000&cpuIterations=8000000
app.MapGet("/benchmark", ([FromQuery] int? arraySize, [FromQuery] int? cpuIterations) =>
{
    int a = arraySize ?? 100000;
    int c = cpuIterations ?? 5000000;

    Console.WriteLine("Starting REST benchmark");

    var start = DateTime.UtcNow;

    // Memory work
    var data = CreateArray(a);
    var sum = SequentialSum(data);

    // CPU work
    var checksum = CpuBurn(c, sum);

    var elapsedSeconds = (DateTime.UtcNow - start).TotalSeconds;

    Console.WriteLine($"Array size: {a}");
    Console.WriteLine($"CPU iterations: {c}");
    Console.WriteLine($"Elapsed seconds: {elapsedSeconds}");

    var result = new BenchmarkResult(
        status: "ok",
        arraySize: a,
        cpuIterations: c,
        checksum: checksum,
        elapsedSeconds: elapsedSeconds
    );

    return Results.Json(result);
});

app.Run();

static int[] CreateArray(int size)
{
    var arr = new int[size];
    for (int i = 0; i < size; i++)
    {
        arr[i] = i;
    }
    return arr;
}

static int SequentialSum(int[] arr)
{
    int sum = 0;
    for (int i = 0; i < arr.Length; i++)
    {
        sum += arr[i];
    }
    return sum;
}

static int CpuBurn(int iterations, int seed)
{
    int x = seed;
    for (int i = 0; i < iterations; i++)
    {
        x = (x * 31 + i) & 0x7fffffff;
    }
    return x;
}

public sealed record BenchmarkResult(
    string status,
    int arraySize,
    int cpuIterations,
    int checksum,
    double elapsedSeconds
);

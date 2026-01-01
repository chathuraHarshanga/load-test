var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:9090");
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

app.MapGraphQL("/graphql");

app.Run();

public sealed class Query
{
    public BenchmarkResult Benchmark(int arraySize = 100000, int cpuIterations = 5000000)
    {
        Console.WriteLine("Starting GraphQL benchmark");

        var start = DateTime.UtcNow;

        var data = CreateArray(arraySize);
        var sum = SequentialSum(data);

        var checksum = CpuBurn(cpuIterations, sum);

        var elapsedSeconds = (DateTime.UtcNow - start).TotalSeconds;

        Console.WriteLine($"Array size: {arraySize}");
        Console.WriteLine($"CPU iterations: {cpuIterations}");
        Console.WriteLine($"Elapsed seconds: {elapsedSeconds}");

        return new BenchmarkResult("ok", arraySize, cpuIterations, checksum, elapsedSeconds);
    }

    private static int[] CreateArray(int size)
    {
        var arr = new int[size];
        for (int i = 0; i < size; i++)
        {
            arr[i] = i;
        }
        return arr;
    }

    private static int SequentialSum(int[] arr)
    {
        int sum = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            sum += arr[i];
        }
        return sum;
    }

    private static int CpuBurn(int iterations, int seed)
    {
        int x = seed;
        for (int i = 0; i < iterations; i++)
        {
            x = (x * 31 + i) & 0x7fffffff;
        }
        return x;
    }
}

public sealed record BenchmarkResult(
    string status,
    int arraySize,
    int cpuIterations,
    int checksum,
    double elapsedSeconds
);

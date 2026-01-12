import ballerina/graphql;
import ballerina/io;
import ballerina/time;

type BenchmarkResult record {|
    string status;
    int arraySize;
    int cpuIterations;
    int checksum;
    decimal elapsedSeconds;
|};

service / on new graphql:Listener(9090) {

    resource function get benchmark(int arraySize = 100000, int cpuIterations = 5000000) returns BenchmarkResult|error {
        io:println("Starting benchmark");

        time:Utc startTime = time:utcNow();

        // Memory work
        int[] data = createArray(arraySize);
        int sum = sequentialSum(data);

        // CPU work
        int cpuResult = cpuBurn(cpuIterations, sum);

        time:Utc endTime = time:utcNow();
        decimal elapsed = time:utcDiffSeconds(endTime, startTime);

        io:println(string `Array size: ${arraySize}`);
        io:println(string `CPU iterations: ${cpuIterations}`);
        io:println(string `Elapsed seconds: ${elapsed}`);

        return {
            status: "ok",
            arraySize: arraySize,
            cpuIterations: cpuIterations,
            checksum: cpuResult,
            elapsedSeconds: elapsed
        };
    }
}

function createArray(int size) returns int[] {
    int[] arr = [];
    int i = 0;
    while i < size {
        arr[i] = i;
        i += 1;
    }
    return arr;
}

function sequentialSum(int[] arr) returns int {
    int sum = 0;
    foreach int v in arr {
        sum += v;
    }
    return sum;
}

function cpuBurn(int iterations, int seed) returns int {
    int x = seed;
    int i = 0;
    while i < iterations {
        x = (x * 31 + i) & 0x7fffffff;
        i += 1;
    }
    return x;
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DFDExam.App.Data;
using DFDExam.App.Benchmarks;
using DotNetRunner = BenchmarkDotNet.Running.BenchmarkRunner;

#region Setup Configuration for Stopwatch
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
var sqlConnectionString = config["SqlServer:ConnectionString"]!;
var sqloptions = new DbContextOptionsBuilder<SqlDbContext>().UseSqlServer(sqlConnectionString, sqlServerOptions =>
{
    sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(10),
        errorNumbersToAdd: null);
}).Options;

using var sqlcontext = new SqlDbContext(sqloptions);
// Retry loop to ensure database is created before running benchmarks
const int maxRetries = 10;
const int delayMs = 2000;
for (int i = 0; i < maxRetries; i++)
{
    try
    {
        Console.WriteLine("Attempting to connect to SQL Server...");
        sqlcontext.Database.EnsureCreated();
        Console.WriteLine("Connected to SQL Server and ensured database is created.");
        break; // Exit loop if successful
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Attempt {i + 1} to connect to SQL Server failed: {ex.Message}");
        if (i == maxRetries - 1) throw;
        await Task.Delay(delayMs);
    }
}
var mongoContext = new MongoContext(
    config["MongoDB:ConnectionString"]!,
    config["MongoDB:Database"]!);

var runner = new BenchmarkRunner(sqlcontext, mongoContext);
await runner.RunAsync();
#endregion

#region BenchmarkDotNet Execution
// Uncomment the following line to run BenchmarkDotNet benchmarks and outcomment the Stopwatch section
DotNetRunner.Run<DatabaseBenchmarks>();
#endregion
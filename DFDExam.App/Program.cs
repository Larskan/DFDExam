using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DFDExam.App.Data;
using DFDExam.App.Benchmarks;

#region Setup Configuration for Stopwatch
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var sqloptions = new DbContextOptionsBuilder<SqlDbContext>().UseSqlServer(config["SqlServer:ConnectionString"]).Options;

using var sqlcontext = new SqlDbContext(sqloptions);
var mongoContext = new MongoContext(
    config["MongoDB:ConnectionString"]!,
    config["MongoDB:Database"]!);

var runner = new BenchmarkRunner(sqlcontext, mongoContext);
await runner.RunAsync();
#endregion

#region BenchmarkDotNet Execution
// Uncomment the following line to run BenchmarkDotNet benchmarks and outcomment the Stopwatch section
// BenchmarkRunner.Run<DatabaseBenchmarks>();
#endregion
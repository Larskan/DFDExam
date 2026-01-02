using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DFDExam.App.Data;
using DFDExam.App.Benchmarks;

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
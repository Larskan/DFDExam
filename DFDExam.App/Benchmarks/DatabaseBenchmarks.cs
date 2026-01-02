using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DFDExam.App.Data;
using DFDExam.App.Models;
using DFDExam.App.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DFDExam.App.Benchmarks;

[MemoryDiagnoser] // To measure memory usage
public class DatabaseBenchmarks
{
    private SqlDbContext _sqlContext = null!;
    private MongoContext _mongoContext = null!;
    private const int UserCount = 10000;

    [GlobalSetup]
    public async Task Setup()
    {
        // SQL Setup
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        var sqlOptions = new DbContextOptionsBuilder<SqlDbContext>()
            .UseSqlServer(config["SqlServer:ConnectionString"]).Options;

        _sqlContext = new SqlDbContext(sqlOptions);
        _sqlContext.Users.RemoveRange(_sqlContext.Users); // Clean up before benchmark
        await _sqlContext.SaveChangesAsync();

        // MongoDB Setup
        _mongoContext = new MongoContext(
            config["MongoDB:ConnectionString"]!,
            config["MongoDB:Database"]!);
        await _mongoContext.Users.DeleteManyAsync(_ => true);
    }

    [Benchmark(Description = "SQL Insert 10k Users")]
    public async Task BenchmarkSqlInsert()
    {
        var users = UserGenerator.GenerateSqlUsers(UserCount);
        await _sqlContext.Users.AddRangeAsync(users);
        await _sqlContext.SaveChangesAsync();
    }

    [Benchmark(Description = "MongoDB Insert 10k Users")]
    public async Task BenchmarkMongoInsert()
    {
        // Note: BenchmarkDotNet does not support async benchmarks directly.
        // This is a workaround to call async method synchronously.
        var users = UserGenerator.GenerateMongoUsers(UserCount);
        _mongoContext.Users.InsertManyAsync(users).GetAwaiter().GetResult();
    }

    [Benchmark(Description = "SQL Read All Users")]
    public void SqlReadAll() => _sqlContext.Users.ToList();

    [Benchmark(Description = "MongoDB Read All Users")]
    public void MongoReadAll() => _mongoContext.Users.Find(_ => true).ToList();

}
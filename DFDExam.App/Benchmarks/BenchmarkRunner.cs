using System.Diagnostics;
using MongoDB.Driver;
using DFDExam.App.Data;
using DFDExam.App.Utility;
using Microsoft.EntityFrameworkCore;

namespace DFDExam.App.Benchmarks;

public class BenchmarkRunner
{
    private readonly SqlDbContext _sqlDbContext;
    private readonly MongoContext _mongoContext;
    const int userCount = 10000;

    public BenchmarkRunner(SqlDbContext sqlDbContext, MongoContext mongoContext)
    {
        _sqlDbContext = sqlDbContext;
        _mongoContext = mongoContext;
    }

    public async Task RunAsync()
    {
        await WarmUpAsync();
        await CleanupAsync();
        Console.WriteLine("Starting benchmarks...\n");
        await BenchmarkSqlInsert(userCount);
        await BenchmarkMongoInsert(userCount);
        await SqlReadAllUsers();
        await MongoReadAllUsers();
    }

    private async Task BenchmarkSqlInsert(int count)
    {
        var users = UserGenerator.GenerateSqlUsers(count);
        var stopwatch = Stopwatch.StartNew();

        _sqlDbContext.Users.AddRange(users);
        await _sqlDbContext.SaveChangesAsync();

        stopwatch.Stop();
        Console.WriteLine($"SQL Insert: Inserted {count} users in {stopwatch.ElapsedMilliseconds} ms");
    }

    private async Task BenchmarkMongoInsert(int count)
    {
        var users = UserGenerator.GenerateMongoUsers(count);
        var stopwatch = Stopwatch.StartNew();

        await _mongoContext.Users.InsertManyAsync(users);

        stopwatch.Stop();
        Console.WriteLine($"MongoDB Insert: Inserted {count} users in {stopwatch.ElapsedMilliseconds} ms");
    }

    private async Task SqlReadAllUsers()
    {
        var stopwatch = Stopwatch.StartNew();

        var users = await _sqlDbContext.Users.ToListAsync();

        stopwatch.Stop();
        Console.WriteLine($"SQL Read: Retrieved {users.Count} users in {stopwatch.ElapsedMilliseconds} ms");
    }

    private async Task MongoReadAllUsers()
    {
        var stopwatch = Stopwatch.StartNew();

        var users = await _mongoContext.Users.Find(_ => true).ToListAsync();

        stopwatch.Stop();
        Console.WriteLine($"MongoDB Read: Retrieved {users.Count} users in {stopwatch.ElapsedMilliseconds} ms");
    }

    private async Task WarmUpAsync()
    {
        // Prevent JIT compilation and cold start effects
        await _sqlDbContext.Users.Take(1).ToListAsync();
        await _mongoContext.Users.Find(_ => true).Limit(1).ToListAsync();
    }

    private async Task CleanupAsync()
    {
        // Clean up previous test data
        _sqlDbContext.Users.RemoveRange(_sqlDbContext.Users);
        await _sqlDbContext.SaveChangesAsync();

        await _mongoContext.Users.DeleteManyAsync(_ => true);
    }
}
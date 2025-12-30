using MongoDB.Driver;
using DFDExam.App.Models;

namespace DFDExam.App.Data;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<MongoUser> Users => _database.GetCollection<MongoUser>("Users");
}
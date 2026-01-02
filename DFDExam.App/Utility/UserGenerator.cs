using DFDExam.App.Models;

namespace DFDExam.App.Utility;

public static class UserGenerator
{
    public static List<SqlUser> GenerateSqlUsers(int count)
    {
        return Enumerable.Range(1, count).Select(i => new SqlUser
        {
            Name = $"User {i}",
            Email = $"user{i}@example.com"
        }).ToList();
    }

    public static List<MongoUser> GenerateMongoUsers(int count)
    {
        return Enumerable.Range(1, count).Select(i => new MongoUser
        {
            Name = $"User {i}",
            Email = $"user{i}@example.com"
        }).ToList();
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DFDExam.App.Models;

public class MongoUser
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}


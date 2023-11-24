using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Company
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; }

    [BsonElement("Address")]
    public string Address { get; set; }

    [BsonElement("Phone")]
    public string Phone { get; set; }

    [BsonElement("Email")]
    public string Email { get; set; }

    [BsonElement("Website")]
    public string Website { get; set; }

    [BsonElement("Description")]
    public string Description { get; set; }

    [BsonElement("Logo")]
    public string Logo { get; set; }
}

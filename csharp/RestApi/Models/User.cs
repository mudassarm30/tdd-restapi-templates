using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RestApi.Repositories;

namespace RestApi.Models;

[BsonCollection("users")]
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Username")]
    public string Username { get; set; }

    [JsonIgnore]
    [BsonElement("Password")]
    public string Password { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; }

    [BsonElement("Address")]
    public string Address { get; set; }

    [BsonElement("City")]
    public string City { get; set; }

    [BsonElement("State")]
    public string State { get; set; }

    [BsonElement("Country")]
    public string Country { get; set; }

    [BsonElement("PostalCode")]
    public string PostalCode { get; set; }

    [BsonElement("Phone")]
    public string Phone { get; set; }

    [BsonElement("Email")]
    public string Email { get; set; }
}
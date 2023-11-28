using System.Reflection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace RestApi.Repositories;

public class DatabaseInitializer
{
    private readonly IMongoClient _mongoClient;
    private readonly MongoDBConfig _config;

    public DatabaseInitializer(IMongoClient mongoClient, MongoDBConfig config)
    {
        _mongoClient = mongoClient;
        _config = config;
    }

    public static string GetCollectionName<T>()
    {
        var collectionAttribute = (BsonCollectionAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(BsonCollectionAttribute));
        return collectionAttribute?.CollectionName ?? typeof(T).Name.ToLower();
    }

    public async Task SyncDatabaseWithModels()
    {
        var database = _mongoClient.GetDatabase(_config.DatabaseName);

        // Get all model types
        var modelTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.Namespace != null && t.Namespace.StartsWith("RestApi.Models") && !t.Namespace.EndsWith("Payloads") && !t.Namespace.EndsWith("Responses") && t.IsClass);

        var collectionNames = new List<string>();

        // Ensure collections exist for all model types
        foreach (var type in modelTypes)
        {
            var collectionAttribute = (BsonCollectionAttribute)Attribute.GetCustomAttribute(type, typeof(BsonCollectionAttribute));
            var collectionName = collectionAttribute?.CollectionName ?? type.Name.ToLower();
            var filter = new BsonDocument("name", collectionName);
            var options = new ListCollectionsOptions { Filter = filter };
            var collectionExists = database.ListCollections(options).ToEnumerable().Any();

            collectionNames.Add(collectionName);

            if (!collectionExists)
            {
                await database.CreateCollectionAsync(collectionName);
            }
        }

        // Delete collections that don't have a corresponding model
        var collections = database.ListCollections().ToEnumerable().ToList();
        foreach (var collection in collections)
        {
            var collectionName = collection["name"].AsString;
            if (!collectionNames.Any(t => t == collectionName))
            {
                await database.DropCollectionAsync(collectionName);
            }
        }
    }
}
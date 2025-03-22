using Inpost_org.Users;
using Inpost_org.Users.Deliveries;
using MongoDB.Driver;
using MongoDB.Bson;
using dotenv.net;

namespace Inpost_org.Services;

public interface IMongoDBService
{
    public void Connect();
    public void ListCollections();
}

/// <summary>
/// Service class for MongoDB operations.
/// </summary>
public class MongoDBService : IMongoDBService
{
    private string _connectionString;
    private MongoClient _client;
    private IMongoDatabase _database;
    public IMongoCollection<PersonModel> collectionUsers;
    public IMongoCollection<ParcelModel> collectionParcels;

    public string DatabaseName { get; private set; }
    public string CollectionNameUsers { get; private set; }
    public string CollectionNameParcels { get; private set; }
    public string DatabaseUser { get; private set; }
    public string DatabasePassword { get; private set; }
    
    /// <summary>
    /// Initializes a new instance of the MongoDBService class.
    /// </summary>
    /// <param name="databaseUser">Database user name.</param>
    /// <param name="databasePassword">Database user password.</param>
    public MongoDBService()
    {
        DatabaseName = "Inpost";
        CollectionNameUsers = "Users";
        CollectionNameParcels = "Parcels";
        try
        {
            DotEnv.Load();
            DatabaseUser = Environment.GetEnvironmentVariable("DATABASE_USER");
            DatabasePassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
            Console.WriteLine($"Database user '{DatabaseUser}' and Password '{DatabasePassword}'");
            _connectionString = $"mongodb+srv://{DatabaseUser}:{DatabasePassword}@datacluster.kcry9.mongodb.net/?retryWrites=true&w=majority&appName=dataCluster";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Missing database user credentials file !!!");
            Console.WriteLine(ex.Message);
            System.Environment.Exit(-1);
        }
    }
    
    ///<summary>
    /// Connects to the MongoDB database.
    /// </summary>
    /// <returns>True if connection is successful, otherwise false.</returns>
    public void Connect()
    {
        try
        {
            _client = new MongoClient(_connectionString);
            _database = _client.GetDatabase(DatabaseName);
            collectionUsers = _database.GetCollection<PersonModel>(CollectionNameUsers);
            collectionParcels = _database.GetCollection<ParcelModel>(CollectionNameParcels);

            Console.WriteLine("Pinging your deployment...");
            var result = _client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Console.WriteLine($"Pinged your deployment. You successfully connected to MongoDB! {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Wrong database user credentials !!!");
            Console.WriteLine($"Could not connect to MongoDB: the connection string is not valid.");
            System.Environment.Exit(-1);
        }
    }

    /// <summary>
    /// Lists all collections in the database.
    /// </summary>
    public void ListCollections()
    {
        var collections = _database.ListCollections().ToList();
        Console.WriteLine("Collections:");
        foreach (var collection in collections)
        {
            Console.WriteLine(collection["name"]);
        }
    }
}
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Inpost_org.Services;

/// <summary>
/// Service class for MongoDB operations.
/// </summary>
public class MongoDBService
{
    private string _connectionString;
    private MongoClient _client;
    private IMongoDatabase _database;
    public IMongoCollection<PersonModel> collectionUsers;
    public IMongoCollection<ParcelModel> collectionParcels;

    public string DatabaseName { get; private set; } = "Inpost";
    public string CollectionNameUsers { get; private set; } = "Users";
    public string CollectionNameParcels { get; private set; } = "Parcels";

    public string DatabaseUser { get; private set; }
    public string DatabasePassword { get; private set; }
    
    /// <summary>
    /// Initializes a new instance of the MongoDBService class.
    /// </summary>
    /// <param name="databaseUser">Database user name.</param>
    /// <param name="databasePassword">Database user password.</param>
    public MongoDBService(string databaseUser, string databasePassword)
    {
        SetUser(databaseUser, databasePassword);
    }
    
    ///<summary>
    /// Sets the database user credentials.
    /// </summary>
    /// <param name="databaseUser">Database user name.</param>
    /// <param name="databasePassword">Database user password.</param>
    public void SetUser(string databaseUser, string databasePassword)
    {
        DatabaseUser = databaseUser;
        DatabasePassword = databasePassword;
        _connectionString = $"mongodb+srv://{DatabaseUser}:{DatabasePassword}@datacluster.kcry9.mongodb.net/?retryWrites=true&w=majority&appName=dataCluster";
    }
    
    ///<summary>
    /// Connects to the MongoDB database.
    /// </summary>
    /// <returns>True if connection is successful, otherwise false.</returns>
    public bool Connect()
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
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not connect to MongoDB: {ex.Message}");
            return false;
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
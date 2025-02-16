using Inpost_org.Users;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Inpost_org.Services;

public class MongoDBService
{
    public string ConnectionString { get; set; }
    
    public MongoClient Client { get; set; }
    public IMongoDatabase Database { get; set; }
    public IMongoCollection<PersonModel> Collection { get; set; }
    
    public string DatabaseName = "Inpost";
    public string CollectionName = "Users";
    
    public string DatabaseUser { get; set; }
    public string DatabasePassword { get; set; }


    public MongoDBService(string databaseUser, string databasePassword)
    {
        DatabaseUser = databaseUser;
        DatabasePassword = databasePassword;
        ConnectionString = $"mongodb+srv://{databaseUser}:{databasePassword}@datacluster.kcry9.mongodb.net/?retryWrites=true&w=majority&appName=dataCluster";
    }

    public void SetUser(string databaseUser, string databasePassword)
    {
        DatabaseUser = databaseUser;
        DatabasePassword = databasePassword;
        ConnectionString = $"mongodb+srv://{databaseUser}:{databasePassword}@datacluster.kcry9.mongodb.net/?retryWrites=true&w=majority&appName=dataCluster";
    }

    public bool Connect()
    {
        try {
            Client = new MongoClient(ConnectionString);
            Database = Client.GetDatabase(DatabaseName);
            Collection = Database.GetCollection<PersonModel>(CollectionName);
            Console.WriteLine("Pinging your deployment...");
            var result = Client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Console.WriteLine($"Pinged your deployment. You successfully connected to MongoDB! {result}");
            return true;
        } catch (Exception ex) {
            Console.WriteLine("Could not connect to MongoDB.");
            return false;
        }
    }
}
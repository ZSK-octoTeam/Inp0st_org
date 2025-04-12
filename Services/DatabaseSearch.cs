using System.Security.Cryptography;
using System.Text;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inpost_org.Services;

public class DatabaseSearch
{
    public static MongoDBService mongo;
     
    /// <summary>
    /// Finds every user that is in the database and returns them into a dictionary
    /// with username for key and PersonModel for value.
    /// </summary>
    /// <returns>dictionary<string, PersonModel></returns>
    public static Dictionary<string, PersonModel> FindUsers()
    {
        Dictionary<string, PersonModel> users = new Dictionary<string, PersonModel>();
            foreach (var databasePerson in mongo.collectionUsers.Find(new BsonDocument()).ToList())
            {
                users.Add(databasePerson.Username, databasePerson);
            }
            
            return users;
    }
    
    /// <summary>
    /// Finds every parcel that is in the database and returns them into a dictionary
    /// with parcel name for key and ParcelModel for value.
    /// </summary>
    /// <returns>Dictionary<string, ParcelModel></returns>
    public static Dictionary<string, ParcelModel> FindParcels()
    {
        Dictionary<string ,ParcelModel> parcels = new Dictionary<string, ParcelModel>();
        foreach (var databaseParcel in mongo.collectionParcels.Find(new BsonDocument()).ToList())
        {
            parcels.Add(databaseParcel.ParcelName, databaseParcel);    
        }
        
        return parcels;
    }

    /// <summary>
    /// Function for hashing passwords with SHA-256
    /// </summary>
    /// <param name="password"></param>
    /// <returns>string hashedPassword</returns>
    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
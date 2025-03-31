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
    /// Sorts users by their roles and the returns them in a dictionary
    /// </summary>
    /// <param name="person"></param>
    /// <returns>dictonary<Role, PersonModel></returns>
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
    /// Returns all parcels that are connected with the user
    /// </summary>
    /// <param name="person"></param>
    /// <returns>List of Parcel(ParcelModel)</returns>
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
    /// function for hashing passwords
    /// </summary>
    /// <param name="password"></param>
    /// <returns>hashed password</returns>
    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
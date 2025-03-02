using System.Security.Cryptography;
using System.Text;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inpost_org.Services;

public class PassphraseMenager
{
    public static MongoDBService mongo;

    /// <summary>
    /// Check if the user is in the database
    /// </summary>
    /// <param name="person"></param>
    /// <returns>true if there is a person with the same username in database</returns>
    public static bool VerifyUser(PersonModel person)
    {
        foreach (var databasePerson in mongo.collectionUsers.Find(new BsonDocument()).ToList())
        {
            if (databasePerson.Username == person.Username)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if user has already a parcel named like that
    /// </summary>
    /// <param name="parcel"></param>
    /// <returns>true if he has</returns>
    public static bool FindParcel(ParcelModel parcel)
    {
        foreach (var databaseParcel in mongo.collectionParcels.Find(new BsonDocument()).ToList())
        {
            if (databaseParcel.ParcelName == parcel.ParcelName &&
                databaseParcel.Recipient.Username == parcel.Recipient.Username)
            {
                return true;
            }
        }

        return false;
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
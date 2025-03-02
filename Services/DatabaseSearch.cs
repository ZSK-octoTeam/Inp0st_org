using System.Security.Cryptography;
using System.Text;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace Inpost_org.Services;

public class DatabaseSearch
{
    public static MongoDBService mongo;

    /// <summary>
    /// Check if the user is in the database
    /// </summary>
    /// <param name="person"></param>
    /// <returns>true if there is a person with the same username in database</returns>
    public static bool FindUser(PersonModel person)
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
    public static bool FindParcel(ParcelModel parcel, PersonModel person)
    {
        foreach (var databaseParcel in mongo.collectionParcels.Find(new BsonDocument()).ToList())
        {
            if (databaseParcel.ParcelName == parcel.ParcelName &&
            person.Username == "adminUser")
            {
                return true;   
            }
            if (databaseParcel.ParcelName == parcel.ParcelName &&
                (databaseParcel.Recipient.Username == person.Username ||
                 databaseParcel.Sender.Username == person.Username))
            {
                return true;
            }
        }
        return false;
    }
     
    /// <summary>
    /// Sorts users by their roles and the returns them in a dictionary
    /// </summary>
    /// <param name="person"></param>
    /// <returns>dictonary<Role, PersonModel></returns>
    public static Dictionary<Role, PersonModel> FindUsers()
    {
        Dictionary<Role, PersonModel> users = new Dictionary<Role, PersonModel>();
            foreach (var databasePerson in mongo.collectionUsers.Find(new BsonDocument()).ToList())
            {
                if (databasePerson.Roles.Contains(Role.Administrator))
                {
                    users.Add(Role.Administrator, databasePerson);
                }else if (databasePerson.Roles.Contains(Role.InpostEmployee))
                {
                    users.Add(Role.InpostEmployee, databasePerson);
                }else if (databasePerson.Roles.Contains(Role.InpostClient))
                {
                    users.Add(Role.InpostClient, databasePerson);
                }
            }

        return users.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
    
    /// <summary>
    /// Returns all parcels that are connected with the user
    /// </summary>
    /// <param name="person"></param>
    /// <returns>List of Parcel(ParcelModel)</returns>
    public static Dictionary<ParcelStatus,ParcelModel> FindParcels(PersonModel person)
    {
        Dictionary<ParcelStatus,ParcelModel> parcels = new Dictionary<ParcelStatus, ParcelModel>();
        {
            if (person.Username == "adminUser")
            {
                foreach (var databaseParcel in mongo.collectionParcels.Find(new BsonDocument()).ToList())
                {
                    if (databaseParcel.Status == ParcelStatus.Delivered)
                    {
                        parcels.Add(ParcelStatus.Delivered, databaseParcel);
                    }
                    else if (databaseParcel.Status == ParcelStatus.InTransport)
                    {
                        parcels.Add(ParcelStatus.InTransport, databaseParcel);
                    }
                    else if (databaseParcel.Status == ParcelStatus.InWarehouse)
                    {
                        parcels.Add(ParcelStatus.InWarehouse, databaseParcel);
                    }
                }
            }
            else
            {
                foreach (var databaseParcel in mongo.collectionParcels.Find(new BsonDocument()).ToList())
                {
                    if (databaseParcel.Recipient.Username == person.Username ||
                        databaseParcel.Sender.Username == person.Username)
                    {
                        if (databaseParcel.Status == ParcelStatus.Delivered)
                        {
                            parcels.Add(ParcelStatus.Delivered, databaseParcel);
                        }
                        else if (databaseParcel.Status == ParcelStatus.InTransport)
                        {
                            parcels.Add(ParcelStatus.InTransport, databaseParcel);
                        }
                        else if (databaseParcel.Status == ParcelStatus.InWarehouse)
                        {
                            parcels.Add(ParcelStatus.InWarehouse, databaseParcel);
                        }
                    }
                }
            }
        }
        return parcels.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
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
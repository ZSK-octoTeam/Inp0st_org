using System.Security.Cryptography;
using System.Text;
using Inpost_org.Users;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inpost_org.Services;

public class PassphraseMenager
{
    public static MongoDBService mongo;
    public static event Action<string, bool> PassphraseVerified;

    public static bool VerifyUser(PersonModel person)
    {
        foreach (var databasePerson in mongo.Collection.Find(new BsonDocument()).ToList())
        {
            if (databasePerson.Username == person.Username)
            {
                PassphraseVerified?.Invoke(person.Username, true);
                return true;
            }
        }
        PassphraseVerified?.Invoke(person.Username, false);
        return false;
    }

    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
using System.Security.Cryptography;
using System.Text;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services;

public class PassphraseMenager
{
    public static MongoDBService mongo { get; set; }
    public static event Action<string, bool> PasswordVerified;

    public static bool VerifyPassword(string username, string password, PersonModel person)
    {
        string hashedPassword = HashPassword(password);
        if (person.Username == username && person.Password == hashedPassword)
        {
            PasswordVerified?.Invoke(username, true);
            return true;
        }
        PasswordVerified?.Invoke(username, false);
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
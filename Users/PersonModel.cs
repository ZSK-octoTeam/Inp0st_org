using Inpost_org.Users.Deliveries;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inpost_org.Users;

public enum Role
{
    Administrator,
    InpostEmployee,
    InpostClient
}

/// <summary>
/// Represents a user in the system, including their credentials and roles.
/// </summary>
public class PersonModel
{
    /// <summary>
    /// The unique identifier for the user, stored as an ObjectId in MongoDB.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Role> Roles { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonModel"/> class with the specified username and password.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    public PersonModel(string username, string password)
    {
        Username = username;
        Password = password;
        Roles = new List<Role>();
    }

    /// <summary>
    /// Adds a role to the user if it is not already assigned.
    /// </summary>
    /// <param name="role">The role to be added.</param>
    public void AddRole(Role role)
    {
        if (!Roles.Contains(role))
        {
            Roles.Add(role);
        }
    }
}
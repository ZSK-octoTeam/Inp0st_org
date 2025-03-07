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

public class PersonModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Role> Roles { get; set; }
    public List<ParcelModel> Parcels { get; set; }
    
    public PersonModel(string username, string password)
    {
        Username = username;
        Password = password;
        Roles = new List<Role>();
        Parcels = new List<ParcelModel>();
    }

    public void AddParcel(ParcelModel parcel)
    {
        if (!Parcels.Contains(parcel))
        {
            Parcels.Add(parcel);
        }
    }
    
    public void AddRole(Role role)
    {
        if (!Roles.Contains(role))
        {
            Roles.Add(role);
        }
    }
}
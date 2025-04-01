using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

public class UpdateUserOperation : UserBase
{
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e, string role)
    {
        e.Operation = "UpdateUser";
        var users = DatabaseSearch.FindUsers();
        if (users.ContainsKey(person.Username))
        {
            string wynik = string.Join(", ", users[person.Username].Roles.Select(e => e.ToString()));
            if (!wynik.Contains(role))
            {
                e.Success = false;
                e.Message = $"User is not a {role}.";
            }
            else
            {
                if (DatabaseSearch.HashPassword(person.Password) == users[person.Username].Password)
                {
                    e.Success = false; 
                    e.Message = "New password is the same as the old one.";
                }
                else
                {
                    var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);
                    var update = Builders<PersonModel>.Update.Set(r => r.Password, DatabaseSearch.HashPassword(person.Password));
                    mongo.collectionUsers.UpdateOne(filter, update);
                    e.Success = true;
                }
            }
        }
        else
        {
            e.Success = false;
            e.Message = "User does not exist.";
        }
    
        OnNotify(person, e);
    }
}
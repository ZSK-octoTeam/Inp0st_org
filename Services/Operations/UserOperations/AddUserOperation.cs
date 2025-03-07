using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

public class AddUserOperation : UserBase
{
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "AddUser";
        var users = DatabaseSearch.FindUsers();
        if (!users.ContainsKey(person.Username))
        {
            person.Password = DatabaseSearch.HashPassword(person.Password);
            mongo.collectionUsers.InsertOne(person);
            e.Success = true;
        }
        else
        {
            e.Success = false;
            e.Message = "User already exists.";
        }

        OnNotify(this, person, e);
    }
}
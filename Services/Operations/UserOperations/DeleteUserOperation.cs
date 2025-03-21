using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

public class DeleteUserOperation : UserBase
{
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "DeleteUser";
        var users = DatabaseSearch.FindUsers();
        if (users.ContainsKey(person.Username))
        {
            var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);
            mongo.collectionUsers.DeleteOne(filter);
            e.Success = true;
        }
        else
        {
            e.Success = false;
            e.Message = "User does not exist.";
        }

        OnNotify(person, e);
    }
}
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations;

public class DeleteUserOperation
{
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public event MongoDBOperationHandler Notify;

    public void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "DeleteUser";
        if (PassphraseMenager.VerifyUser(person))
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

        Notify?.Invoke(this, person, e);
    }
}
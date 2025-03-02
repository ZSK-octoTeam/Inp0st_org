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
        if (PassphraseMenager.VerifyUser(person))
        {
            var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);
            mongo.collectionUsers.DeleteOne(filter);
            Success = true;
            Message = "User deleted.";
        }
        else
        {
            Success = false;
            Message = "User could not be deleted.";
        }

        Notify?.Invoke(this, person, new MongoDBOperationEventArgs("delete", Success, Message));
    }
}
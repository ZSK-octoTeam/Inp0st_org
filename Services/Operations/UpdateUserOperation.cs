using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations;

public class UpdateUserOperation
{
    public bool Success { get; private set; }
    public string Message { get; private set; }
    
    
    public event MongoDBOperationHandler Notify;

    public void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e)
    {
        if (PassphraseMenager.VerifyUser(person))
        {
            var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);
            var update = Builders<PersonModel>.Update.Set(r => r.Password, PassphraseMenager.HashPassword(person.Password));
            mongo.collectionUsers.UpdateOne(filter, update);
            Success = true;
            Message = "User updated.";
        }
        else
        {
            Success = false;
            Message = "User could not be updated.";
        }

        Notify?.Invoke(this, person, new MongoDBOperationEventArgs("update", Success, Message));
    }
}
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

public class UpdateUserOperation : crudUsers
{
    public bool Success { get; private set; }
    public string Message { get; private set; }
    
    
    public event MongoDBUserOperationHandler Notify;

    public void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "UpdateUser";
        var users = DatabaseSearch.FindUsers();
        if (users.ContainsKey(person.Username))
        {
            var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);
            var update = Builders<PersonModel>.Update.Set(r => r.Password, DatabaseSearch.HashPassword(person.Password));
            mongo.collectionUsers.UpdateOne(filter, update);
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
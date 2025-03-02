using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;

namespace Inpost_org.Services.Operations;

public class AddUserOperation : CRUD
{
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public event MongoDBOperationHandler Notify;

    public void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e)
    {
        if (!PassphraseMenager.VerifyUser(person))
        {
            person.Password = PassphraseMenager.HashPassword(person.Password);
            mongo.collectionUsers.InsertOne(person);
            Success = true;
            Message = "User added";
        }
        else
        {
            Success = false;
            Message = "User already exists.";
        }

        Notify?.Invoke(this, person, e);
    }
}
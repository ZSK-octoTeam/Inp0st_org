using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;

namespace Inpost_org.Services.Operations.UserOperations;

public class AddUserOperation : crudUsers
{
    public event MongoDBUserOperationHandler Notify;

    public void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "AddUser";
        if (!PassphraseMenager.VerifyUser(person))
        {
            person.Password = PassphraseMenager.HashPassword(person.Password);
            mongo.collectionUsers.InsertOne(person);
            e.Success = true;
        }
        else
        {
            e.Success = false;
            e.Message = "User already exists.";
        }

        Notify?.Invoke(this, person, e);
    }
}
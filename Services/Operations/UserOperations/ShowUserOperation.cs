using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Inpost_org.Services.Operations.UserOperations;

public class ShowUserOperation : crudUsers
{
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public event MongoDBUserOperationHandler Notify;

    public void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "Show users";
        e.Success = false;
        PersonModel databasePerson = null;
        foreach (var user in mongo.collectionUsers.Find(new BsonDocument()).ToList())
        {
            if (user.Username == person.Username)
            {
                databasePerson = user;
                e.Success = true;
                break;
            }
        }
        
        if (e.Success)
        {
            var rbac = new RBAC();
            e.Operation = "ShowUser";
            e.Message += $"Username: {databasePerson.Username}\n";
            e.Message += "Role: \n";
            foreach (var role in databasePerson.Roles)
            {
                e.Message += $"-{role}\n";
            }

            e.Message += "\nHas permission to: \n";            
            foreach (var permission in Enum.GetValues(typeof(Permission)))
            {
                if (rbac.HasPermission(databasePerson, (Permission)permission))
                {
                    e.Message += $"-{permission}\n";
                }
            }
        }
        else
        {
            e.Success = false;
            e.Message = "User does not exist.";
        }

        Notify?.Invoke(this, databasePerson, e);
    }
}
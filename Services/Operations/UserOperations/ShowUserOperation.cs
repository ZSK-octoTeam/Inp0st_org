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
        e.Operation = "Show user";
        e.Success = false;
        var users = DatabaseSearch.FindUsers();
        
        if (users.ContainsKey(person.Username))
        {
            person = users[person.Username];
            
            var rbac = new RBAC();
            
            e.Operation = "ShowUser";
            e.Message += $"Username: {person.Username}\n";
            e.Message += "Roles: \n";
            
            foreach (var role in person.Roles)
            {
                e.Message += $"-{role}\n";
            }

            e.Message += "\nHas permission to: \n";            
            foreach (var permission in Enum.GetValues(typeof(Permission)))
            {
                if (rbac.HasPermission(person, (Permission)permission))
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

        Notify?.Invoke(this, person, e);
    }
}
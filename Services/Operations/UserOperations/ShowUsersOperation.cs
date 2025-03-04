using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;

namespace Inpost_org.Services.Operations.UserOperations;

public class ShowUsersOperation : crudUsers
{
    public event MongoDBUserOperationHandler Notify;
    
    public void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "Show users";
        e.Message += "Showing users: ";
        foreach (var user in DatabaseSearch.FindUsers())
        {
            e.Message += $"- {user.Key} - roles: \n";
            foreach (var role in user.Value.Roles)
            {
                e.Message += $"- '{role}'\n";
            }
        }
        e.Success = true;
        Notify?.Invoke(this, person, e);
    }
}
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;

namespace Inpost_org.Services.Operations.UserOperations;

public class ShowUsersOperation : UserBase
{
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "Show users";
        e.Message += "Showing users: \n";
        foreach (var user in DatabaseSearch.FindUsers())
        {
            e.Message += $"- {user.Key} - roles: \n";
            if (user.Value.Roles.Count == 0)
            {
                e.Message += $"- 'none'\n";
            }
            else
            {
                foreach (var role in user.Value.Roles)
                {
                    e.Message += $"- '{role}'\n";
                }   
            }
        }
        e.Success = true;
        
        OnNotify(person, e);
    }
}
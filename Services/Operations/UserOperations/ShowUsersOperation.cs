using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;

namespace Inpost_org.Services.Operations.UserOperations;

public class ShowUsersOperation : UserBase
{
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e, string role)
    {
        e.Operation = "Show users";
        e.Message += "Showing users: \n";
        foreach (var user in DatabaseSearch.FindUsers())
        {
            string wynik = string.Join(", ", user.Value.Roles.Select(e => e.ToString()));
            if (wynik.Contains(role))
            {
                e.Message += $"\n- {user.Key} - roles: \n";
                if (user.Value.Roles.Count == 0)
                {
                    e.Message += $"'none'\n";
                }
                else
                {
                    foreach (var r in user.Value.Roles)
                    {
                        e.Message += $"\t'{r}'\n";
                    }
                }
            }
        }
        e.Success = true;
        
        OnNotify(person, e);
    }
}
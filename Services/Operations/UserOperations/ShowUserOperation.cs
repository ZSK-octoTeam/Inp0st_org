using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Inpost_org.Services.Operations.UserOperations;

public class ShowUserOperation : UserBase
{
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e, string role)
    {
        e.Operation = "Show user";
        e.Success = false;
        var users = DatabaseSearch.FindUsers();
        if (!users.ContainsKey(person.Username))
        {
            e.Success = false;
            e.Message = "User does not exist.";
        }
        else
        {
            string wynik = string.Join(", ", users[person.Username].Roles.Select(e => e.ToString()));
            
            if (wynik.Contains(role))
            {
                e.Success = true;

                person = users[person.Username];

                var rbac = new RBAC();

                e.Operation = "ShowUser";
                e.Message += $"Username: {person.Username}\n";
                e.Message += "Roles: \n";

                foreach (var r in person.Roles)
                {
                    e.Message += $"-{r}\n";
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
                e.Message = $"User is not a {role}.";
            }
        }

        OnNotify(person, e);
    }
}
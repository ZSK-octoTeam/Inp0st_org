using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;

namespace Inpost_org.Services.Operations.UserOperations;

/// <summary>
/// Class responsible for displaying all users and their roles based on a specified role filter.
/// </summary>
public class ShowUsersOperation : UserBase
{
    /// <summary>
    /// Executes the operation of displaying all users and their roles filtered by a specified role.
    /// </summary>
    /// <param name="mongo">The MongoDB service object.</param>
    /// <param name="person">The person model representing the user requesting the operation.</param>
    /// <param name="e">The event arguments for the MongoDB operation.</param>
    /// <param name="role">The role used to filter the displayed users.</param>
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
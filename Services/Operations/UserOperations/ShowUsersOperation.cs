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
        // Set the operation type
        e.Operation = "Show users";

        // Initialize the message with a header
        e.Message += "Showing users: \n";

        // Iterate through all users in the database
        foreach (var user in DatabaseSearch.FindUsers())
        {
            // Retrieve the roles of the user as a comma-separated string
            string wynik = string.Join(", ", user.Value.Roles.Select(e => e.ToString()));

            // Check if the user has the specified role
            if (wynik.Contains(role))
            {
                // Append user details to the message
                e.Message += $"\n- {user.Key} - roles: \n";

                // Check if the user has no roles assigned
                if (user.Value.Roles.Count == 0)
                {
                    e.Message += $"'none'\n";
                }
                else
                {
                    // List all roles assigned to the user
                    foreach (var r in user.Value.Roles)
                    {
                        e.Message += $"\t'{r}'\n";
                    }
                }
            }
        }

        // Mark the operation as successful
        e.Success = true;

        // Trigger the notification event for the operation
        OnNotify(person, e);
    }
}
using Inpost_org.Enums;
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Inpost_org.Services.Operations.UserOperations;

/// <summary>
/// Class responsible for displaying user details and their permissions.
/// </summary>
public class ShowUserOperation : UserBase
{
    /// <summary>
    /// Executes the operation of displaying user details and their permissions.
    /// </summary>
    /// <param name="mongo">The MongoDB service object.</param>
    /// <param name="person">The person model representing the user to be displayed.</param>
    /// <param name="e">The event arguments for the MongoDB operation.</param>
    /// <param name="role">The role to be checked for the user.</param>
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e, string role)
    {
        // Set the operation type
        e.Operation = "Show user";

        // Initialize the operation as unsuccessful
        e.Success = false;

        // Retrieve the list of existing users
        var users = DatabaseSearch.FindUsers();

        // Check if the user exists
        if (!users.ContainsKey(person.Username))
        {
            // Mark the operation as unsuccessful and set an error message
            e.Success = false;
            e.Message = "User does not exist.";
        }
        else
        {
            // Retrieve the roles of the user as a comma-separated string
            string wynik = string.Join(", ", users[person.Username].Roles.Select(e => e.ToString()));

            // Check if the user has the specified role
            if (wynik.Contains(role))
            {
                // Mark the operation as successful
                e.Success = true;

                // Retrieve the user details
                person = users[person.Username];

                // Initialize the RBAC (Role-Based Access Control) system
                RBAC rbac = new RBAC();

                // Append user details to the message
                e.Operation = "ShowUser";
                e.Message += $"\nUsername: {person.Username}\n";
                e.Message += "Roles: \n";

                // List all roles assigned to the user
                foreach (var r in person.Roles)
                {
                    e.Message += $"\t-{r}\n";
                }

                // List all permissions the user has
                e.Message += "\nHas permission to: \n";
                foreach (var permission in Enum.GetValues(typeof(Permission)))
                {
                    if (rbac.HasPermission(person, (Permission)permission))
                    {
                        e.Message += $"\t-{permission}\n";
                    }
                }
            }
            else
            {
                // Mark the operation as unsuccessful and set an error message
                e.Success = false;
                e.Message = $"User is not a {role}.";
            }
        }

        // Trigger the notification event for the operation
        OnNotify(person, e);
    }
}
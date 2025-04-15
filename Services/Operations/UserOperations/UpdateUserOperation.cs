using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

/// <summary>
/// Class responsible for updating multiple users' details in the MongoDB database.
/// </summary>
public class UpdateUserOperation : UserBase
{
    /// <summary>
    /// Executes the operation of updating multiple users' details in the database.
    /// </summary>
    /// <param name="mongo">The MongoDB service object.</param>
    /// <param name="person">The person model representing the user requesting the operation.</param>
    /// <param name="e">The event arguments for the MongoDB operation.</param>
    /// <param name="role">The role used to filter or validate the users to be updated.</param>
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e, string role)
    {
        // Set the operation type
        e.Operation = "UpdateUsers";

        // Retrieve the list of existing users
        var users = DatabaseSearch.FindUsers();

        // Initialize the operation as unsuccessful
        e.Success = false;

        // Iterate through all users in the database
        foreach (var user in users)
        {
            // Check if the user has the specified role
            if (user.Value.Roles.Any(r => r.ToString() == role))
            {
                // Check if the new password is different from the old one
                if (DatabaseSearch.HashPassword(person.Password) == user.Value.Password)
                {
                    e.Message += $"User {user.Key}: New password is the same as the old one.\n";
                }
                else
                {
                    // Update the user's password in the database
                    var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, user.Key);
                    var update = Builders<PersonModel>.Update.Set(r => r.Password, DatabaseSearch.HashPassword(person.Password));
                    mongo.collectionUsers.UpdateOne(filter, update);

                    // Mark the operation as successful for this user
                    e.Message += $"User {user.Key}: Password updated successfully.\n";
                    e.Success = true;
                }
            }
            else
            {
                // Log a message if the user does not have the specified role
                e.Message += $"User {user.Key}: Does not have the role {role}.\n";
            }
        }

        // Trigger the notification event for the operation
        OnNotify(person, e);
    }
}
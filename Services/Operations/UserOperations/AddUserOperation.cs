using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

/// <summary>
/// Class responsible for adding users to the MongoDB database.
/// </summary>
public class AddUserOperation : UserBase
{
    /// <summary>
    /// Executes the operation of adding a user to the database.
    /// </summary>
    /// <param name="mongo">The MongoDB service object.</param>
    /// <param name="person">The person model representing the user to be added.</param>
    /// <param name="e">The event arguments for the MongoDB operation.</param>
    /// <param name="role">The role to be assigned to the user.</param>
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e, string role)
    {
        // Set the operation type
        e.Operation = "AddUser";

        // Retrieve the list of existing users
        var users = DatabaseSearch.FindUsers();

        // Check if the user does not already exist
        if (!users.ContainsKey(person.Username))
        {
            // Hash the user's password
            person.Password = DatabaseSearch.HashPassword(person.Password);

            // If no roles are assigned, add the provided role
            if (person.Roles.Count == 0)
            {
                person.Roles.Add(Enum.Parse<Role>(role));
            }

            // Insert the new user into the MongoDB collection
            mongo.collectionUsers.InsertOne(person);

            // Mark the operation as successful
            e.Success = true;
        }
        else
        {
            // Retrieve the roles of the existing user as a comma-separated string
            string wynik = string.Join(", ", person.Roles.Select(e => e.ToString()));

            // Check if the user already has the specified role
            if (wynik.Contains(role) || users[person.Username].Roles.Contains(Enum.Parse<Role>(role)))
            {
                // Mark the operation as unsuccessful and set an error message
                e.Success = false;
                e.Message = "User already exists.";
            }
            else
            {
                // Add the new role to the existing user
                users[person.Username].Roles.Add(Enum.Parse<Role>(role));

                // Create a filter to find the user in the database
                var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);

                // Create an update definition to modify the user's roles
                var update = Builders<PersonModel>.Update.Set(r => r.Roles, users[person.Username].Roles);

                // Update the user's roles in the MongoDB collection
                mongo.collectionUsers.UpdateOne(filter, update);

                // Mark the operation as successful and set a success message
                e.Success = true;
                e.Message = $"Role: {role} added to user {person.Username}";
            }
        }

        // Trigger the notification event for the operation
        OnNotify(person, e);
    }
}
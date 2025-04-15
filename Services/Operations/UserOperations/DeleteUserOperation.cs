using Inpost_org.Enums;
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Services.Operations.ParcelOperations;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

/// <summary>
/// Class responsible for deleting users from the MongoDB database.
/// </summary>
public class DeleteUserOperation : UserBase
{
    /// <summary>
    /// Executes the operation of deleting a user from the database.
    /// </summary>
    /// <param name="mongo">The MongoDB service object.</param>
    /// <param name="person">The person model representing the user to be deleted.</param>
    /// <param name="e">The event arguments for the MongoDB operation.</param>
    /// <param name="role">The role to be checked or removed from the user.</param>
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e, string role)
    {
        // Set the operation type
        e.Operation = "DeleteUser";

        // Retrieve the list of existing users
        var users = DatabaseSearch.FindUsers();

        // Check if the user exists
        if (users.ContainsKey(person.Username))
        {
            // Retrieve the roles of the user as a comma-separated string
            string wynik = string.Join(", ", users[person.Username].Roles.Select(e => e.ToString()));

            // Check if the user does not have the specified role
            if (!wynik.Contains(role))
            {
                // Mark the operation as unsuccessful and set an error message
                e.Success = false;
                e.Message = $"User is not a {role}.";
            }
            else
            {
                // Create an instance of DeleteParcelOperation for parcel deletion
                DeleteParcelOperation delPackage = new DeleteParcelOperation();
                delPackage.Notify += EventListener.OnParcelOperation;

                // Check if the user has only one role or the role is invalid
                if (users[person.Username].Roles.Count == 1 || !Enum.TryParse<Role>(role, out var result))
                {
                    // Delete the user from the MongoDB collection
                    var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);
                    mongo.collectionUsers.DeleteOne(filter);
                    e.Success = true;

                    // Delete or update parcels associated with the user
                    foreach (var databaseParcel in DatabaseSearch.FindParcels())
                    {
                        if (databaseParcel.Value.Recipient != null && databaseParcel.Value.Recipient.Username == person.Username)
                        {
                            delPackage.Operation(mongo, databaseParcel.Value, person, new MongoDBOperationEventArgs());
                        }

                        if (databaseParcel.Value.Sender != null && databaseParcel.Value.Sender.Username == person.Username)
                        {
                            var fill = Builders<ParcelModel>.Filter.Eq(r => r.Sender.Username, person.Username);
                            var upt = Builders<ParcelModel>.Update.Set(r => r.Sender, null);
                            mongo.collectionParcels.UpdateOne(fill, upt);
                        }
                    }
                }
                else
                {
                    // Remove the specified role from the user
                    users[person.Username].Roles.Remove(Enum.Parse<Role>(role));
                    var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);
                    var update = Builders<PersonModel>.Update.Set(r => r.Roles, users[person.Username].Roles);
                    mongo.collectionUsers.UpdateOne(filter, update);
                    e.Success = true;
                    e.Message = $"Role: {role} removed from user {person.Username}";

                    // Handle parcel updates based on the removed role
                    foreach (var databaseParcel in DatabaseSearch.FindParcels())
                    {
                        if (Enum.Parse<Role>(role) == Role.InpostClient)
                        {
                            if (databaseParcel.Value.Recipient != null && databaseParcel.Value.Recipient.Username == person.Username)
                            {
                                delPackage.Operation(mongo, databaseParcel.Value, person, new MongoDBOperationEventArgs());
                            }
                        }

                        if (Enum.Parse<Role>(role) == Role.InpostEmployee)
                        {
                            if (databaseParcel.Value.Sender != null && databaseParcel.Value.Sender.Username == person.Username)
                            {
                                var fill = Builders<ParcelModel>.Filter.Eq(r => r.Sender.Username, person.Username);
                                var upt = Builders<ParcelModel>.Update.Set(r => r.Sender, null);
                                mongo.collectionParcels.UpdateOne(fill, upt);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            // Mark the operation as unsuccessful and set an error message
            e.Success = false;
            e.Message = "User does not exist.";
        }

        // Trigger the notification event for the operation
        OnNotify(person, e);
    }
}
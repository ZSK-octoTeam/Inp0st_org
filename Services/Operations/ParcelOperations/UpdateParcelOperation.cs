using Inpost_org.Enums;
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.ParcelOperations;

/// <summary>
/// Class responsible for updating the details of a parcel in the MongoDB database.
/// </summary>
public class UpdateParcelOperation : ParcelBase
{
    /// <summary>
    /// Executes the operation of updating the details of a parcel, such as its sender and status.
    /// </summary>
    /// <param name="mongo">The MongoDB service object.</param>
    /// <param name="parcel">The parcel model representing the parcel to be updated.</param>
    /// <param name="person">The person model representing the user performing the operation.</param>
    /// <param name="e">The event arguments for the MongoDB operation.</param>
    public override void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        // Set the operation type
        e.Operation = "UpdateParcel";

        // Initialize the operation as unsuccessful
        e.Success = false;

        // Check if the parcel exists in the database
        foreach (var userParcel in DatabaseSearch.FindParcels())
        {
            if (userParcel.Key == parcel.ParcelName)
            {
                e.Success = true;
                break;
            }
        }

        // Retrieve the list of users from the database
        var databaseUsers = DatabaseSearch.FindUsers();
        
        // Validate the sender's existence in the database
        if (databaseUsers.ContainsKey(parcel.Sender.Username) == false)
        {
            e.Success = false;
            e.Message = $"{parcel.Sender.Username} does not exist";
        }
        else if (e.Success)
        {
            // Check if the sender exists and has the appropriate role
            foreach (var databaseUser in databaseUsers)
            {
                if (parcel.Sender.Username == databaseUser.Value.Username)
                {
                    if (databaseUser.Value.Roles.Contains(Role.InpostEmployee))
                    {
                        // Update the parcel's sender and status in the database
                        var filter = Builders<ParcelModel>.Filter.Eq(r => r.ParcelName, parcel.ParcelName);
                        var update = Builders<ParcelModel>.Update
                            .Set(r => r.Sender, databaseUser.Value)
                            .Set(r => r.Status, parcel.Status);
                        mongo.collectionParcels.UpdateOne(filter, update);
                    }
                    else
                    {
                        e.Success = false;
                        e.Message = $"{parcel.Sender.Username} does not have permission to deliver packages";
                    }
                }
            }
        }
        else
        {
            // Set an error message if the parcel does not exist
            e.Message = $"Parcel: {parcel.ParcelName} does not exist in the database\n";
        }

        // Trigger the notification event for the operation
        OnNotify(parcel, person, e);
    }
}
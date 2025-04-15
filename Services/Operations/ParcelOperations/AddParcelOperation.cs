using Inpost_org.Enums;
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;
using MongoDB.Driver.Linq;

namespace Inpost_org.Services.Operations.ParcelOperations;

/// <summary>
/// Class responsible for adding a new parcel to the MongoDB database.
/// </summary>
public class AddParcelOperation : ParcelBase
{
    /// <summary>
    /// Executes the operation of adding a new parcel to the database.
    /// </summary>
    /// <param name="mongo">The MongoDB service object.</param>
    /// <param name="parcel">The parcel model representing the parcel to be added.</param>
    /// <param name="person">The person model representing the user performing the operation.</param>
    /// <param name="e">The event arguments for the MongoDB operation.</param>
    public override void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        // Set the operation type
        e.Operation = "AddParcel";

        // Initialize the operation as successful
        e.Success = true;

        // Check if a parcel with the same name already exists
        foreach (var userParcel in DatabaseSearch.FindParcels())
        {
            if (userParcel.Key == parcel.ParcelName)
            {
                e.Success = false;
                e.Message = $"There is already a package named: {parcel.ParcelName}";
                break;
            }
        }

        // Retrieve the list of users from the database
        var databaseUsers = DatabaseSearch.FindUsers();
        
        // Check if the user exists in the database
        if (databaseUsers.ContainsKey(person.Username))
        {
            e.Success = false;
            e.Message = $"User: {person.Username} does not exist";
        }
        else if (e.Success)
        {
            // Check if the recipient exists and has the appropriate role
            foreach (var databaseUser in databaseUsers)
            {
                if (parcel.Recipient.Username == databaseUser.Value.Username)
                {
                    if (databaseUser.Value.Roles.Contains(Role.InpostClient))
                    {
                        // Add the parcel to the database
                        mongo.collectionParcels.InsertOne(parcel);
                        e.Success = true;
                    }
                    else
                    {
                        e.Success = false;
                        e.Message = $"User: {databaseUser.Value.Username} does not have permission to receive parcels";
                    }
                }
            }
        }

        // Trigger the notification event for the operation
        OnNotify(parcel, person, e);
    }
}
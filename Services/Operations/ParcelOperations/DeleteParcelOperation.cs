using Inpost_org.Enums;
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.ParcelOperations;

/// <summary>
/// Class responsible for deleting a parcel from the MongoDB database.
/// </summary>
public class DeleteParcelOperation : ParcelBase
{
    /// <summary>
    /// Executes the operation of deleting a parcel from the database.
    /// </summary>
    /// <param name="mongo">The MongoDB service object.</param>
    /// <param name="parcel">The parcel model representing the parcel to be deleted.</param>
    /// <param name="person">The person model representing the user performing the operation.</param>
    /// <param name="e">The event arguments for the MongoDB operation.</param>
    public override void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        // Set the operation type
        e.Operation = "DeleteParcel";

        // Initialize the operation as unsuccessful
        e.Success = false;

        // Iterate through the parcels in the database
        foreach (var userParcel in DatabaseSearch.FindParcels())
        {
            // Check if the parcel exists and the user is authorized to delete it
            if (userParcel.Key == parcel.ParcelName && (userParcel.Value.Recipient.Username == person.Username || person.Roles.Contains(Role.Administrator)))
            {
                e.Success = true;
                break;
            }
        }

        if (e.Success)
        {
            // Delete the parcel from the database
            var filter = Builders<ParcelModel>.Filter.Eq(r => r.ParcelName, parcel.ParcelName);
            mongo.collectionParcels.DeleteOne(filter);
        }
        else if (DatabaseSearch.FindParcels().ContainsKey(parcel.ParcelName) == false)
        {
            // Set an error message if the parcel does not exist
            e.Message = $"Parcel: {parcel.ParcelName} doesn't exist";
        }
        else
        {
            // Set an error message if the user is not authorized to delete the parcel
            e.Message = $"User: {person.Username} doesn't have a parcel named: {parcel.ParcelName}";
        }

        // Trigger the notification event for the operation
        OnNotify(parcel, person, e);
    }
}
using Inpost_org.Enums;
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;

namespace Inpost_org.Services.Operations.ParcelOperations;

/// <summary>
/// Class responsible for displaying details of a specific parcel from the MongoDB database.
/// </summary>
public class ShowParcelOperation : ParcelBase
{
    /// <summary>
    /// Executes the operation of displaying details of a specific parcel.
    /// </summary>
    /// <param name="mongo">The MongoDB service object.</param>
    /// <param name="parcel">The parcel model representing the parcel to be displayed.</param>
    /// <param name="person">The person model representing the user performing the operation.</param>
    /// <param name="e">The event arguments for the MongoDB operation.</param>
    public override void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "ShowParcel";
        e.Success = false;
        foreach (var userParcel in DatabaseSearch.FindParcels())
        {
            if (userParcel.Key == parcel.ParcelName && (userParcel.Value.Recipient.Username == person.Username || person.Roles.Contains(Role.Administrator) || (userParcel.Value.Sender != null && userParcel.Value.Sender.Username == person.Username)))
            {
                parcel = userParcel.Value;
                e.Success = true;
                break;
            }
        }

        if(DatabaseSearch.FindParcels().ContainsKey(parcel.ParcelName) == false)
        {
            e.Success = false;
            e.Message = $"Parcel: {parcel.ParcelName} does not exist in the database\n";
        }
        else if (e.Success)
        {
           e.Message += $"Parcel name: {parcel.ParcelName}\n";
           e.Message += $"Parcel status: {parcel.Status}\n";
           e.Message += $"Parcel owner: {parcel.Recipient.Username}\n";
           if (parcel.Sender == null)
           {
               e.Message += $"Parcel deliverer: none\n";
           }
           else
           {
               e.Message += $"Parcel deliverer: {parcel.Sender.Username}\n";
           }
        }
        else
        {
            e.Success = false;
            e.Message += $"User: {person.Username} does not have a parcel called: {parcel.ParcelName}\n";
        }

        OnNotify(parcel, person, e);
    }
}
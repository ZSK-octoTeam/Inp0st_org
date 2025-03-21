using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;

namespace Inpost_org.Services.Operations.ParcelOperations;

public class ShowParcelOperation : ParcelBase
{
    public override void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "ShowParcel";
        e.Success = true;
        foreach (var userParcel in DatabaseSearch.FindParcels())
        {
            if (userParcel.Key == parcel.ParcelName && userParcel.Value.Recipient.Username == person.Username)
            {
                parcel = userParcel.Value;
                e.Success = false;
                break;
            }
        }

        if (!e.Success)
        {
           e.Success = true;
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
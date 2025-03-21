using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

public class ShowParcelsOperation : UserBase
{
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "Show parcels";
        e.Message += "Showing parcels: \n";
        foreach (var databaseParcel in DatabaseSearch.FindParcels())
        {
            if (databaseParcel.Value.Recipient.Username == person.Username)
            {
                e.Message += $"Parcel name: {databaseParcel.Value.ParcelName}, parcel status: {databaseParcel.Value.Status}, you are reciver\n";
            }

            if (databaseParcel.Value.Sender.Username == person.Username)
            {
                e.Message += $"Parcel name: {databaseParcel.Value.ParcelName}, parcel status: {databaseParcel.Value.Status}, you are sender\n";
            }
        }
        e.Success = true;

        OnNotify(person, e);
    }
}
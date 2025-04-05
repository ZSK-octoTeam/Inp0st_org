using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;
using MongoDB.Driver.Linq;

namespace Inpost_org.Services.Operations.ParcelOperations;

public class AddParcelOperation : ParcelBase
{
    public override void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "AddParcel";
        e.Success = true;
        foreach (var userParcel in DatabaseSearch.FindParcels())
        {
            if (userParcel.Key == parcel.ParcelName && userParcel.Value.Recipient.Username == person.Username)
            {
                e.Success = false;
                break;
            }
        }

        if(DatabaseSearch.FindUsers().ContainsKey(person.Username) == false)
        {
            e.Success = false;
            e.Message = $"User: {person.Username} does not exist";
        }
        else if (e.Success)
        {
            mongo.collectionParcels.InsertOne(parcel);
        }
        else
        {
            e.Message = $"User: {person.Username} already has a parcel named: {parcel.ParcelName}";
        }

        OnNotify(parcel, person, e);
    }
}
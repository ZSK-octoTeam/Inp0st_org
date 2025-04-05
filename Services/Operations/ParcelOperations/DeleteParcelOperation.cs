using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.ParcelOperations;

public class DeleteParcelOperation : ParcelBase
{
    public override void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "DeleteParcel";
        e.Success = false;
        foreach (var userParcel in DatabaseSearch.FindParcels())
        {
            if (userParcel.Key == parcel.ParcelName && (userParcel.Value.Recipient.Username == person.Username || person.Roles.Contains(Role.Administrator)))
            {
                e.Success = true;
                break;
            }
        }

        if (e.Success)
        {
            var filter = Builders<ParcelModel>.Filter.Eq(r => r.ParcelName, parcel.ParcelName);
            mongo.collectionParcels.DeleteOne(filter);
        }
        else if(DatabaseSearch.FindParcels().ContainsKey(parcel.ParcelName) == false)
        {
            e.Message = $"Parcel: {parcel.ParcelName} doesnt exist";
        }
        else
        {
            e.Message = $"User: {person.Username} doesnt have a parcel named: {parcel.ParcelName}";
        }

        OnNotify(parcel, person, e);
    }
}
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.ParcelOperations;

public class UpdateParcelOperation : ParcelBase
{
    public override void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        
        //repair
        e.Operation = "UpdateParcel";
        e.Success = false;
        foreach (var userParcel in person.Parcels)
        {
            if (parcel.Id == userParcel.Id)
            {
                e.Success = true;
                break;
            }
        }

        if (e.Success)
        {
            var filter = Builders<ParcelModel>.Filter.Eq(r => r.Id, parcel.Id);
            var update = Builders<ParcelModel>.Update
                .Set(r => r.ParcelName, parcel.ParcelName)
                .Set(r => r.Recipient, parcel.Recipient)
                .Set(r => r.Sender, parcel.Sender)
                .Set(r => r.Status, parcel.Status);
            mongo.collectionParcels.UpdateOne(filter, update);
        }
        else
        {
            e.Message = $"User: {person.Username} does not have a parcel called: {parcel.ParcelName}\n";
        }

        OnNotify(parcel, person, e);
    }    
}
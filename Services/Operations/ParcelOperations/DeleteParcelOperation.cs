using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.ParcelOperations;

public class DeleteParcelOperation : crudParcels
{
    public event MongoDBParcelOperationHandler Notify;

    public void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "DeleteParcel";
        e.Success = true;
        foreach (var userParcel in person.Parcels)
        {
            if (userParcel.ParcelName == parcel.ParcelName)
            {
                e.Success = false;
                break;
            }
        }

        if (!e.Success)
        {
            var filter = Builders<ParcelModel>.Filter.Eq(r => r.ParcelName, parcel.ParcelName);
            mongo.collectionParcels.DeleteOne(filter);
            e.Success = true;
        }
        else
        {
            e.Success = false;
            e.Message = $"User: {person.Username} already has a parcel named: {parcel.ParcelName}";
        }
        
        Notify?.Invoke(this, parcel, person, e);
    }
}
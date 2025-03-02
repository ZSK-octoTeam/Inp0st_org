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
        if (DatabaseSearch.FindParcel(parcel, person))
        {
            var filter = Builders<ParcelModel>.Filter.Eq(r => r.Id, parcel.Id);
            mongo.collectionParcels.DeleteOne(filter);
            e.Success = true;
        }else
        {
            e.Success = false;
            e.Message = $"User: {person.Username} doesn't have parcel called: {parcel.ParcelName}.";
        }
        
        Notify?.Invoke(this, parcel, person, e);
    }
}
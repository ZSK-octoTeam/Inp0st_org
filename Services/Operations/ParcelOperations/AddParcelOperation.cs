using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;

namespace Inpost_org.Services.Operations.ParcelOperations;

public class AddParcelOperation : crudParcels
{
    public event MongoDBParcelOperationHandler Notify;

    public void Operation(MongoDBService mongo, ParcelModel parcel, MongoDBOperationEventArgs e)
    {
        e.Operation = "AddParcel";
        if (!PassphraseMenager.FindParcel(parcel))
        {
            mongo.collectionParcels.InsertOne(parcel);
            e.Success = true;
        }
        else
        {
            e.Success = false;
            e.Message = $"User: {parcel.Recipient.Username} already has a parcel named: {parcel.ParcelName}";
        }
        
        Notify?.Invoke(this, parcel, e);
    }
}
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;

namespace Inpost_org.Services.Operations.ParcelOperations;

public class AddParcelOperation : crudParcels
{
    public event MongoDBParcelOperationHandler Notify;

    public void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "AddParcel";
        if (!DatabaseSearch.FindParcel(parcel, person))
        {
            mongo.collectionParcels.InsertOne(parcel);
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
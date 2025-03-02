using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;

namespace Inpost_org.Services.Operations.ParcelOperations;

public class ShowParcelOperation : crudParcels
{
    public event MongoDBParcelOperationHandler Notify;

    public void Operation(MongoDBService mongo, ParcelModel parcel, MongoDBOperationEventArgs e)
    {
        
    }
}
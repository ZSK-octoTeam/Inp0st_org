using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;

namespace Inpost_org.Services.Operations.ParcelOperations;

public class ShowParcelOperation : crudParcels
{
    public event MongoDBParcelOperationHandler Notify;
    public PersonModel person { get; set; }
    
    public void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "ShowParcel";
        
    }
}
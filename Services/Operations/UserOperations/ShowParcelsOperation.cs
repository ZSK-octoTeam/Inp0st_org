using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

public class ShowParcelsOperation : crudUsers
{
    public event MongoDBUserOperationHandler Notify;
    
    public void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "Show parcels";
        e.Message += "Showing parcels: ";
        foreach (var databaseParcel in DatabaseSearch.FindParcels())
        {
            if (person.Parcels.Contains(databaseParcel.Value))
            {
                e.Message += $"Parcel name: {databaseParcel.Value.ParcelName}, parcel status: {databaseParcel.Value.Status}, parcel reciver: {person.Username}\n";
            }
        }
        e.Success = true;
        Notify?.Invoke(this, person, e);
    }
}
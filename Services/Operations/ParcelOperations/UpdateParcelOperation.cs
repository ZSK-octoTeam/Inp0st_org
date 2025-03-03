using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.ParcelOperations;

public class UpdateParcelOperation : crudParcels
{
    public event MongoDBParcelOperationHandler Notify;
    
    public void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        
        //repair
        e.Operation = "UpdateParcel";
        var userParcels = DatabaseSearch.FindParcels();
        if (userParcels.ContainsValue(parcel))
        {
            var filter = Builders<ParcelModel>.Filter.Eq(r => r.Recipient.Username, person.Username);
            var update = Builders<ParcelModel>.Update.Set(r => r.Status, parcel.Status);
            mongo.collectionParcels.UpdateOne(filter, update);
            e.Success = true;
        }
        else
        {
            e.Success = false;
            e.Message = $"User: {person.Username} does not have a parcel named: {parcel.ParcelName}";
        }
        
        Notify?.Invoke(this, parcel, person, e);
    }    
}
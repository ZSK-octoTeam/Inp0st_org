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
        foreach (var userParcel in DatabaseSearch.FindParcels())
        {
            if (userParcel.Key == parcel.ParcelName)
            {
                if(parcel.Recipient.Username == "")
                {
                    parcel.Recipient = userParcel.Value.Recipient;
                }
                e.Success = true;
                break;
            }
        }

        if(DatabaseSearch.FindUsers().ContainsKey(parcel.Recipient.Username) == false || DatabaseSearch.FindUsers().ContainsKey(parcel.Sender.Username) == false)
        {
            e.Success = false;
            e.Message = $"Recipient or  sender does not exist";
        }
        else if (e.Success)
        {
            var filter = Builders<ParcelModel>.Filter.Eq(r => r.Id, parcel.Id);
            var update = Builders<ParcelModel>.Update
                .Set(r => r.Sender, parcel.Sender)
                .Set(r => r.Status, parcel.Status)
                .Set(r => r.Recipient, parcel.Recipient);
            mongo.collectionParcels.UpdateOne(filter, update);
        }
        else
        {
            e.Message = $"Parcel: {parcel.ParcelName} does not exist in the database\n";
        }

        OnNotify(parcel, person, e);
    }    
}
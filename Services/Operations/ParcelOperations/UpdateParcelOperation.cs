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
                e.Success = true;
                break;
            }
        }
        
        
        if(DatabaseSearch.FindUsers().ContainsKey(parcel.Sender.Username) == false)
        {
            e.Success = false;
            e.Message = $"{parcel.Sender.Username} does not exist";
        }
        else if (e.Success)
        {
           
            
            var databaseUsers = DatabaseSearch.FindUsers();
            foreach (var databaseUser in databaseUsers)
            {
                if (parcel.Sender.Username == databaseUser.Value.Username)
                {
                    if (databaseUser.Value.Roles.Contains(Role.InpostEmployee))
                    {
                        var filter = Builders<ParcelModel>.Filter.Eq(r => r.ParcelName, parcel.ParcelName);
                        var update = Builders<ParcelModel>.Update
                            .Set(r => r.Sender, databaseUser.Value)
                            .Set(r => r.Status, parcel.Status);
                        mongo.collectionParcels.UpdateOne(filter, update);
                    }
                    else
                    {
                        e.Success = false;
                        e.Message = $"{parcel.Sender.Username} does not have permission to deliver packages";
                    }
                }
            }
        }
        else
        {
            e.Message = $"Parcel: {parcel.ParcelName} does not exist in the database\n";
        }

        OnNotify(parcel, person, e);
    }    
}
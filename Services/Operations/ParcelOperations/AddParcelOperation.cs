using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;
using MongoDB.Driver.Linq;

namespace Inpost_org.Services.Operations.ParcelOperations;

public class AddParcelOperation : ParcelBase
{
    public override void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "AddParcel";
        e.Success = true;
        foreach (var userParcel in DatabaseSearch.FindParcels())
        {
            if (userParcel.Key == parcel.ParcelName)
            {
                e.Success = false;
                break;
            }
        }

        if(DatabaseSearch.FindUsers().ContainsKey(person.Username) == false)
        {
            e.Success = false;
            e.Message = $"User: {person.Username} does not exist";
        }
        else if (e.Success)
        {
            var databaseUsers = DatabaseSearch.FindUsers();
            foreach (var databaseUser in databaseUsers)
            {
                if (parcel.Recipient.Username == databaseUser.Value.Username)
                {
                    if (databaseUser.Value.Roles.Contains(Role.InpostClient))
                    {
                        mongo.collectionParcels.InsertOne(parcel);
                        e.Success = true;
                    }
                    else
                    {
                        e.Success = false;
                        e.Message = $"User: {databaseUser.Value.Username} does hot have a permission to receive parcel";
                    }
                }
            }
            
        }
        else
        {
            e.Message = $"There is already a package named: {parcel.ParcelName}";
        }

        OnNotify(parcel, person, e);
    }
}
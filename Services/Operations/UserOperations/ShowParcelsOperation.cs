using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

public class ShowParcelsOperation : UserBase
{
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e, string role)
    {
        e.Operation = "Show parcels";
        e.Message += "Showing parcels: \n";
        var databaseParcels = DatabaseSearch.FindParcels();
            foreach (var databaseParcel in databaseParcels)
            {
                 if (databaseParcel.Value.Recipient.Username == person.Username || databaseParcel.Value.Sender.Username == person.Username || person.Roles.Contains(Role.Administrator) || role == "InpostEmployeeAll")
                  {
                      e.Message += $"Parcel name: {databaseParcel.Value.ParcelName}, parcel status: {databaseParcel.Value.Status}, parcel sender: {databaseParcel.Value.Sender.Username}, parcel reciever: {databaseParcel.Value.Recipient.Username}\n";   }
            }
            e.Success = true;
        }
        OnNotify(person, e);
    }
}
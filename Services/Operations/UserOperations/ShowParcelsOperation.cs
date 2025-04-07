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
            try
            {
                string sender = databaseParcel.Value.Sender == null ? "N/A" : databaseParcel.Value.Sender.Username;
                string reciver = databaseParcel.Value.Recipient == null ? "N/A" : databaseParcel.Value.Recipient.Username;
                if (sender == person.Username || reciver == person.Username || person.Roles.Contains(Role.Administrator) || role == "InpostEmployeeAll")
                {
                    
                    e.Message += $"Parcel name: {databaseParcel.Value.ParcelName}, parcel status: {databaseParcel.Value.Status}, parcel sender: {sender}, parcel reciever: {reciver}\n";
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        e.Success = true;
        OnNotify(person, e);
    }
}
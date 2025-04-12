using Inpost_org.Enums;
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

/// <summary>
/// Class responsible for displaying parcels associated with a user or accessible by their role.
/// </summary>
public class ShowParcelsOperation : UserBase
{
    /// <summary>
    /// Executes the operation of displaying parcels associated with the specified user or accessible by their role.
    /// </summary>
    /// <param name="mongo">The MongoDB service object.</param>
    /// <param name="person">The person model representing the user requesting the operation.</param>
    /// <param name="e">The event arguments for the MongoDB operation.</param>
    /// <param name="role">The role of the user requesting the operation in case of Admin.</param>
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
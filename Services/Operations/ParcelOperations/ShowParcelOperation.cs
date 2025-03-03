using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;

namespace Inpost_org.Services.Operations.ParcelOperations;

public class ShowParcelOperation : crudParcels
{
    public event MongoDBParcelOperationHandler Notify;
    
    public void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        e.Operation = "ShowParcel";
        var userParcels = DatabaseSearch.FindParcels(person);
        if (userParcels.ContainsValue(parcel))
        {
            var databaseParcel = parcel;
            foreach (var iterator in userParcels)
            {
                if (iterator.Value.ParcelName == parcel.ParcelName)
                {
                    databaseParcel = iterator.Value;
                    break;
                }
            }
            e.Success = true;
            e.Message += "Info about parcel: \n";
            e.Message += $"Parcel name: {databaseParcel.ParcelName}\n";
            e.Message += $"Parcel reciver: {databaseParcel.Recipient}\n";
            e.Message += $"Parcel status: {databaseParcel.Status}\n";
        }
        else
        {
            e.Success = false;
            e.Message = $"User {person.Username} does not have parcel: {parcel.ParcelName}";
        }
        
        Notify?.Invoke(this, parcel, person, e);
    }
}
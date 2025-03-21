using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;

namespace Inpost_org.Services;

public abstract class UserBase
{
    public event MongoDBUserOperationHandler Notify;

    protected void OnNotify(PersonModel person, MongoDBOperationEventArgs e)
    {
        Notify?.Invoke(this, person, e);
    }
    
    public abstract void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e);
}

public abstract class ParcelBase
{
    public event MongoDBParcelOperationHandler Notify;
    protected void OnNotify(ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        Notify?.Invoke(this, parcel, person, e);
    }
    
    public abstract void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e);
}
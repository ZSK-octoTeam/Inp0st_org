using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;

namespace Inpost_org.Services;

public abstract class UserBase
{

    private event MongoDBUserOperationHandler _notify;
    public event MongoDBUserOperationHandler Notify
    {
        add => _notify += value;
        remove => _notify -= value;
    }

    protected void OnNotify(object sender, PersonModel person, MongoDBOperationEventArgs e)
    {
        _notify?.Invoke(sender, person, e);
    }
    
    public abstract void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e);
}

public abstract class ParcelBase
{
    private event MongoDBParcelOperationHandler _notify;
    public event MongoDBParcelOperationHandler Notify
    {
        add => _notify += value;
        remove => _notify -= value;
    }

    protected void OnNotify(object sender, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        _notify?.Invoke(sender, parcel, person, e);
    }
    
    public abstract void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e);
}
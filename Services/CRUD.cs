using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;

namespace Inpost_org.Services;

public interface crudUsers
{
    void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e);
}

public interface crudParcels
{
    void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e);
}
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;

namespace Inpost_org.Services.NotificationMethods;

public delegate void MongoDBUserOperationHandler(object sender, PersonModel person, MongoDBOperationEventArgs e);
public delegate void MongoDBParcelOperationHandler(object sender, ParcelModel parcel, MongoDBOperationEventArgs e);
using Inpost_org.Users;

namespace Inpost_org.Services.NotificationMethods;

public delegate void MongoDBOperationHandler(object sender, PersonModel person, MongoDBOperationEventArgs e);
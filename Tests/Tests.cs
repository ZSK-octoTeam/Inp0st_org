using Inpost_org.Services.Operations.ParcelOperations;
using Inpost_org.Services.Operations.UserOperations;
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Services.Operations;
using Inpost_org.Services;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;

namespace Inpost_org.Tests;

public class Tests
{
    public static void TestAddUserOperation(MongoDBService mongo)
    {
        try
        {
            AddUserOperation add = new AddUserOperation();
            add.Notify += EventListener.OnUserOperation;

            DeleteUserOperation del = new DeleteUserOperation();
            del.Notify += EventListener.OnUserOperation;

            PersonModel person = new PersonModel("Admin", "passAdmin");
            
            add.Operation(mongo, person, new MongoDBOperationEventArgs(), "Administrator");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
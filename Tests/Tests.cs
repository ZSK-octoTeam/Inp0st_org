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

            PersonModel person = new PersonModel("user1", "haslo1");
            person.AddRole(Role.InpostEmployee);
            
            PersonModel person1 = new PersonModel("user2", "haslo");
            person.AddRole(Role.InpostClient);
            
            PersonModel person2 = new PersonModel("user3", "haslo");
            person.AddRole(Role.Administrator);
            
            add.Operation(mongo, person, new MongoDBOperationEventArgs(), "");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
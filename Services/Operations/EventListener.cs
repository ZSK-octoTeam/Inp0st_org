using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;

namespace Inpost_org.Services.Operations;

public class EventListener
{
    public static void OnUserOperation(object sender, PersonModel person, MongoDBOperationEventArgs e)
    {
        string action = e.Success ? "success" : "failure";
        Console.WriteLine($"Operation '{e.Operation}' completed for user: {person.Username}, with: {action}.");
        Console.WriteLine($"{e.Message}");
    }

    public static void OnParcelOperation(object sender, ParcelModel parcel, MongoDBOperationEventArgs e)
    {
        string action = e.Success ? "success" : "failure";
        Console.WriteLine($"Operation '{e.Operation}' completed for parcel: {parcel.ParcelName}, with {action}");
        Console.WriteLine($"{e.Message}");
    }
}
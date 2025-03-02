using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;

namespace Inpost_org.Services.Operations;

public class EventListener
{
    public static void OnOperation(object sender, PersonModel person, MongoDBOperationEventArgs e)
    {
        string action = e.Success ? "success" : "failure";
        Console.WriteLine($"Operation '{e.Operation}' completed for user: {person.Username}, with: {action}.");
        Console.WriteLine($"{e.Message}");
    }
}
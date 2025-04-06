using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;

namespace Inpost_org.Services.Operations;

public class LogManager()
{
    private static readonly string logFilePath = "event_log.txt";
    
    protected static void LogToFile(MongoDBOperationEventArgs e)
    {
        string action = e.Success ? "success" : "failure";
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine($"Event: {e.Operation}, completed with status: {action} on date: {DateTime.Now}\n");
        }
    }
}

public class EventListener : LogManager
{
    public static void OnUserOperation(object sender, PersonModel person, MongoDBOperationEventArgs e)
    {
        string user = person == null ? "Admin" : person.Username;
        if (e.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Operation '{e.Operation}' completed for user: {user}, with success.");
            Console.ResetColor();
            Console.WriteLine(e.Message);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Operation '{e.Operation}' completed for user: {user}, with failure.");
            Console.ResetColor();
            Console.WriteLine(e.Message);
        }
        //LogToFile(e);
    }

    public static void OnParcelOperation(object sender, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        string action = e.Success ? "success" : "failure";
        if (e.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Operation '{e.Operation}' completed for parcel: {parcel.ParcelName}, with status success.");
            Console.ResetColor();
            Console.WriteLine(e.Message);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Operation '{e.Operation}' completed for parcel: {parcel.ParcelName}, with status failure.");
            Console.ResetColor();
            Console.WriteLine(e.Message);
        }
        //LogToFile(e);
    }
}
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;

namespace Inpost_org.Services.Operations;

public class LogManager()
{
    private static readonly string logFilePath = "event_log.txt";
    
    /// <summary>
    /// This method writes message from event to event_log.txt file with current date, time and
    /// status of the operation.
    /// </summary>
    /// <param name="e">MongoDBOperationEventArgs</param>
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
    /// <summary>
    /// Handles user-related operations and logs the result.
    /// Displays a success or failure message in the console with appropriate formatting.
    /// Logs the operation details to a file.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="person">The user involved in the operation.</param>
    /// <param name="e">Event arguments containing operation details and status.</param>
    public static void OnUserOperation(object sender, PersonModel person, MongoDBOperationEventArgs e)
    {
        string user = person == null ? "Admin" : person.Username;
        if (e.Success)
        {
            // Display success message in green
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Operation '{e.Operation}' completed for user: {user}, with success.");
            Console.ResetColor();
            if (e.Message != "")
            {
                Console.WriteLine(e.Message);
            }
        }
        else
        {
            // Display failure message in red
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Operation '{e.Operation}' completed for user: {user}, with failure.");
            Console.ResetColor();
            if (e.Message != "")
            {
                Console.WriteLine(e.Message);
            }
        }
        
        // Log the operation details to a file
        LogToFile(e);
    }

    /// <summary>
    /// Handles parcel-related operations and logs the result.
    /// Displays a success or failure message in the console with appropriate formatting.
    /// Logs the operation details to a file.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="parcel">The parcel involved in the operation.</param>
    /// <param name="person">The user associated with the parcel operation.</param>
    /// <param name="e">Event arguments containing operation details and status.</param>
    public static void OnParcelOperation(object sender, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        string action = e.Success ? "success" : "failure";
        if (e.Success)
        {
            //Display success message in green
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Operation '{e.Operation}' completed for parcel: {parcel.ParcelName}, with status success.");
            Console.ResetColor();
            if (e.Message != "")
            {
                Console.WriteLine(e.Message);
            }
        }
        else
        {
            //Display failure message in red
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Operation '{e.Operation}' completed for parcel: {parcel.ParcelName}, with status failure.");
            Console.ResetColor();
            if (e.Message != "")
            {
                Console.WriteLine(e.Message);
            }
        }
        
        //Log the operation details to a file
        LogToFile(e);
    }
}
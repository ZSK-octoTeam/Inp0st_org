namespace Inpost_org.Services.NotificationMethods;

/// <summary>
/// Represents the event arguments for MongoDB operations.
/// </summary>
public class MongoDBOperationEventArgs : EventArgs
{
    // The name of the operation being performed
    public string Operation { get; set; }

    // Indicates whether the operation was successful
    public bool Success { get; set; }

    // Optional message providing additional details about the operation
    public string Message { get; set; }

    /// <summary>
    /// Default constructor initializing properties with default values.
    /// </summary>
    public MongoDBOperationEventArgs()
    {
        Operation = "";
        Success = false;
        Message = "";
    }
}
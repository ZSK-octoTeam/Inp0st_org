namespace Inpost_org.Services.NotificationMethods;

/// <summary>
/// Represents the event arguments for MongoDB operations.
/// </summary>
public class MongoDBOperationEventArgs : EventArgs
{
    public string Operation { get; set; }
    public bool Success { get; set; }
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
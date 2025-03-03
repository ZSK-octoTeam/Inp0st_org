namespace Inpost_org.Services.NotificationMethods;

public class MongoDBOperationEventArgs : EventArgs
{
    public string Operation { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    
    public MongoDBOperationEventArgs()
    {
        Operation = "";
        Success = false;
        Message = "";
    }
}
namespace Inpost_org.Services.NotificationMethods;

public class MongoDBOperationEventArgs
{
    public string Operation { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    
    public MongoDBOperationEventArgs(string operation, bool success, string message)
    {
        Operation = operation;
        Success = success;
        if (Success)
        {
            Message = $"with success, {message}";
        }else
        {
            Message = $"with failure, {message}";
        }
    }
}
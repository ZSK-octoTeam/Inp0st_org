using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inpost_org.Users.Deliveries;

public enum ParcelStatus
{
    InWarehouse,
    InTransport,
    Delivered
}

public class ParcelModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ParcelName { get; set; }
    public PersonModel Sender { get; set; }
    public PersonModel Recipient { get; set; }
    public ParcelStatus Status { get; set; }
    
    public ParcelModel(string parcelName, PersonModel recipient)
    {
        ParcelName = parcelName;
        Recipient = recipient;
        Status = ParcelStatus.InWarehouse;
    }
    
    public void ChangeStatus(ParcelStatus newStatus)
    {
        Status = newStatus;
    }

    public void ChangeSender(PersonModel newSender)
    {
        Sender = newSender;
    }
}
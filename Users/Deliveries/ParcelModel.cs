using Inpost_org.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inpost_org.Users.Deliveries;

/// <summary>
/// Represents a parcel in the system, including its name, sender, recipient, and status.
/// </summary>
public class ParcelModel
{
    /// <summary>
    /// The unique identifier for the parcel, stored as an ObjectId in MongoDB.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ParcelName { get; set; }
    public PersonModel Sender { get; set; }
    public PersonModel Recipient { get; set; }
    public ParcelStatus Status { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParcelModel"/> class with the specified name and recipient.
    /// </summary>
    /// <param name="parcelName">The name of the parcel.</param>
    /// <param name="recipient">The recipient of the parcel.</param>
    public ParcelModel(string parcelName, PersonModel recipient)
    {
        ParcelName = parcelName;
        Recipient = recipient;
        Status = ParcelStatus.InWarehouse;
    }

    /// <summary>
    /// Changes the status of the parcel.
    /// </summary>
    /// <param name="newStatus">The new status to be assigned to the parcel.</param>
    public void ChangeStatus(ParcelStatus newStatus)
    {
        Status = newStatus;
    }

    /// <summary>
    /// Changes the sender of the parcel.
    /// </summary>
    /// <param name="newSender">The new sender to be assigned to the parcel.</param>
    public void ChangeSender(PersonModel newSender)
    {
        Sender = newSender;
    }
}
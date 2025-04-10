using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;

namespace Inpost_org.Services.Operations.UserOperations;

/// <summary>
/// Class responsible for displaying parcels associated with a user or accessible by their role.
/// </summary>
public class ShowParcelsOperation : UserBase
{
    /// <summary>
    /// Executes the operation of displaying parcels associated with the specified user or accessible by their role.
    /// </summary>
    /// <param name="mongo">The MongoDB service object.</param>
    /// <param name="person">The person model representing the user requesting the operation.</param>
    /// <param name="e">The event arguments for the MongoDB operation.</param>
    /// <param name="role">The role of the user requesting the operation in case of Admin.</param>
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e, string role)
    {
        // Set the operation type
        e.Operation = "Show parcels";

        // Initialize the message with a header
        e.Message += "Showing parcels: \n";

        // Retrieve the list of parcels from the database
        var databaseParcels = DatabaseSearch.FindParcels();

        // Iterate through the parcels in the database
        foreach (var databaseParcel in databaseParcels)
        {
            try
            {
                // Determine the sender and recipient usernames, or set them to "N/A" if null
                string sender = databaseParcel.Value.Sender == null ? "N/A" : databaseParcel.Value.Sender.Username;
                string receiver = databaseParcel.Value.Recipient == null ? "N/A" : databaseParcel.Value.Recipient.Username;

                // Check if the user is authorized to view the parcel
                if (sender == person.Username || receiver == person.Username || person.Roles.Contains(Role.Administrator) || role == "InpostEmployeeAll")
                {
                    // Append parcel details to the message
                    e.Message += $"Parcel name: {databaseParcel.Value.ParcelName}, parcel status: {databaseParcel.Value.Status}, parcel sender: {sender}, parcel receiver: {receiver}\n";
                }
            }
            catch (Exception exception)
            {
                // Log and rethrow any exceptions encountered
                Console.WriteLine(exception);
                throw;
            }
        }

        // Mark the operation as successful
        e.Success = true;

        // Trigger the notification event for the operation
        OnNotify(person, e);
    }
}
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;

namespace Inpost_org.Services;

/// <summary>
/// Base class for all user operation classes which handles event notifications.
/// </summary>
public abstract class UserBase
{
    /// <summary>
    /// Event triggered to notify about user-related operations.
    /// </summary>
    public event MongoDBUserOperationHandler Notify;

    /// <summary>
    /// Invokes the Notify event with the provided user and operation details.
    /// </summary>
    /// <param name="person">The user involved in the operation.</param>
    /// <param name="e">Event arguments containing operation details and status.</param>
    protected void OnNotify(PersonModel person, MongoDBOperationEventArgs e)
    {
        Notify?.Invoke(this, person, e);
    }
    
    /// <summary>
    /// Abstract method to perform a user-related operation.
    /// Must be implemented by derived classes.
    /// </summary>
    /// <param name="mongo">The MongoDB service instance.</param>
    /// <param name="person">The user involved in the operation.</param>
    /// <param name="e">Event arguments containing operation details and status.</param>
    /// <param name="role">The role associated with the operation.</param>
    public abstract void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e, string role);
}

/// <summary>
/// Base class for all parcel operation classes which handles event notifications.
/// </summary>
public abstract class ParcelBase
{
    /// <summary>
    /// Event triggered to notify about parcel-related operations.
    /// </summary>
    public event MongoDBParcelOperationHandler Notify;
    
    /// <summary>
    /// Invokes the Notify event with the provided parcel, user, and operation details.
    /// </summary>
    /// <param name="parcel">The parcel involved in the operation.</param>
    /// <param name="person">The user associated with the parcel operation.</param>
    /// <param name="e">Event arguments containing operation details and status.</param>
    protected void OnNotify(ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e)
    {
        Notify?.Invoke(this, parcel, person, e);
    }
    
    /// <summary>
    /// Abstract method to perform a parcel-related operation.
    /// Must be implemented by derived classes.
    /// </summary>
    /// <param name="mongo">The MongoDB service instance.</param>
    /// <param name="parcel">The parcel involved in the operation.</param>
    /// <param name="person">The user associated with the parcel operation.</param>
    /// <param name="e">Event arguments containing operation details and status.</param>
    public abstract void Operation(MongoDBService mongo, ParcelModel parcel, PersonModel person, MongoDBOperationEventArgs e);
}
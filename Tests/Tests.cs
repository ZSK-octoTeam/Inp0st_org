using Inpost_org.Services.Operations.ParcelOperations;
using Inpost_org.Services.Operations.UserOperations;
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Services.Operations;
using Inpost_org.Services;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;

namespace Inpost_org.Tests;

public class Tests
{

    public static void PersonOperationsTest(MongoDBService mongo)
    {
        PersonModel user = new PersonModel("user1","haslo1");
        user.AddRole(Role.InpostClient);

        UserBase addUser = new AddUserOperation();
        addUser.Notify += EventListener.OnUserOperation;
    
        addUser.Operation(mongo, user, new MongoDBOperationEventArgs());
        addUser.Operation(mongo, user, new MongoDBOperationEventArgs());
    
        UserBase showUser = new ShowUserOperation();
        showUser.Notify += EventListener.OnUserOperation;
    
        showUser.Operation(mongo, user, new MongoDBOperationEventArgs());
    
        UserBase delUser = new DeleteUserOperation();
        delUser.Notify += EventListener.OnUserOperation;
    
        delUser.Operation(mongo, user, new MongoDBOperationEventArgs());
        delUser.Operation(mongo, user, new MongoDBOperationEventArgs());
        showUser.Operation(mongo, user, new MongoDBOperationEventArgs());
    
        addUser.Operation(mongo, user, new MongoDBOperationEventArgs());
    
        UserBase showUsers = new ShowUsersOperation();
        showUsers.Notify += EventListener.OnUserOperation;
    
        showUsers.Operation(mongo, user, new MongoDBOperationEventArgs());
    
        UserBase uptUser = new UpdateUserOperation();
        uptUser.Notify += EventListener.OnUserOperation;

        user.Password = "haslo2";
        uptUser.Operation(mongo, user, new MongoDBOperationEventArgs());
    
        user.Username = "user2";
        uptUser.Operation(mongo, user, new MongoDBOperationEventArgs());
        user.Username = "user1";

        delUser.Operation(mongo, user, new MongoDBOperationEventArgs());
    }

    public static void ParcelOperationsTest(MongoDBService mongo)
    {
        PersonModel user1 = new PersonModel("user1", "haslo1");
        PersonModel user2 = new PersonModel("user2", "haslo2");
        PersonModel user3 = new PersonModel("user3", "haslo3");
        PersonModel user4 = new PersonModel("user4", "haslo4");
        user1.AddRole(Role.InpostEmployee);
        user2.AddRole(Role.InpostEmployee);
        user3.AddRole(Role.InpostClient);
        user4.AddRole(Role.InpostClient);

        UserBase addUser = new AddUserOperation();
        addUser.Notify += EventListener.OnUserOperation;
        
        addUser.Operation(mongo, user1, new MongoDBOperationEventArgs());
        addUser.Operation(mongo, user2, new MongoDBOperationEventArgs());
        addUser.Operation(mongo, user3, new MongoDBOperationEventArgs());
        addUser.Operation(mongo, user4, new MongoDBOperationEventArgs());

        ParcelModel parcel1 = new ParcelModel("parcel1", user3);
        ParcelModel parcel2 = new ParcelModel("parcel2", user4);
        ParcelModel parcel3 = new ParcelModel("parcel3", user3);
        ParcelModel parcel4 = new ParcelModel("parcel4", user4);
        
        parcel1.ChangeSender(user1);
        parcel2.ChangeSender(user2);
        parcel3.ChangeSender(user1);
        parcel4.ChangeSender(user2);
        
        ParcelBase addParcel = new AddParcelOperation();
        addParcel.Notify += EventListener.OnParcelOperation;
        
        addParcel.Operation(mongo, parcel1, user3, new MongoDBOperationEventArgs());
        addParcel.Operation(mongo, parcel2, user4, new MongoDBOperationEventArgs());
        addParcel.Operation(mongo, parcel3, user3, new MongoDBOperationEventArgs());
        addParcel.Operation(mongo, parcel4, user4, new MongoDBOperationEventArgs());

        UserBase showParcels = new ShowParcelsOperation();
        showParcels.Notify += EventListener.OnUserOperation;
        
        showParcels.Operation(mongo, user3, new MongoDBOperationEventArgs());
        showParcels.Operation(mongo, null, new MongoDBOperationEventArgs());
        
        ParcelBase delParcel = new DeleteParcelOperation();
        delParcel.Notify += EventListener.OnParcelOperation;
        
        delParcel.Operation(mongo, parcel1, user3, new MongoDBOperationEventArgs());
        showParcels.Operation(mongo, user3, new MongoDBOperationEventArgs());
        
        ParcelBase showParcel = new ShowParcelOperation();
        showParcel.Notify += EventListener.OnParcelOperation;
        
        showParcel.Operation(mongo, parcel3, user3, new MongoDBOperationEventArgs());
        
        ParcelBase uptParcel = new UpdateParcelOperation();
        uptParcel.Notify += EventListener.OnParcelOperation;
        
        parcel3.ChangeStatus(ParcelStatus.InTransport);
        uptParcel.Operation(mongo, parcel3, user3, new MongoDBOperationEventArgs());
        
        showParcel.Operation(mongo, parcel3, user3, new MongoDBOperationEventArgs());
    }
}
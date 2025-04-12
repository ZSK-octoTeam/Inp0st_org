using Inpost_org.Enums;
using Inpost_org.Services;
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Services.Operations;
using Inpost_org.Services.Operations.ParcelOperations;
using Inpost_org.Services.Operations.UserOperations;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;

namespace Inpost_org.UI;

public class OperationEngine
{
    public static void ShowClients(PersonModel loggedIn,MongoDBService mongo){
        Inputs.ShowHeader("=== CLIENTS ===");
        ShowUsersOperation showUsers = new ShowUsersOperation();
        showUsers.Notify += EventListener.OnUserOperation;
        showUsers.Operation(mongo, loggedIn, new MongoDBOperationEventArgs(), "InpostClient");
        Console.ReadKey();
    }

    public static void AddClient(MongoDBService mongo)
    {
        Inputs.ShowHeader("=== ADD CLIENT ===");
        AddUserOperation addUser = new AddUserOperation();
        addUser.Notify += EventListener.OnUserOperation;
        
        bool roleAdded = false;
        string username = Inputs.GetInputString("Enter username:");
        PersonModel addedUser = new PersonModel(username, "");
        
        foreach (var databaseUser in DatabaseSearch.FindUsers())
        {
            if (username == databaseUser.Key && !databaseUser.Value.Roles.Contains(Role.InpostClient))
            {
                addUser.Operation(mongo, addedUser, new MongoDBOperationEventArgs(), "InpostClient");
                roleAdded = true;
                break;
            }
        }

        if (!roleAdded)
        {
            addedUser.Password = Inputs.GetPassword("Enter password:");
            addUser.Operation(mongo, addedUser, new MongoDBOperationEventArgs(), "InpostClient");
        }
        
        System.Threading.Thread.Sleep(3500);
    }

    public static void DeleteClient(MongoDBService mongo){
        Inputs.ShowHeader("=== DELETE CLIENT ===");
        DeleteUserOperation deleteUser = new DeleteUserOperation();
        deleteUser.Notify += EventListener.OnUserOperation;
        PersonModel deletedUser = new PersonModel(Inputs.GetInputString("Enter username:"), "");
        deleteUser.Operation(mongo, deletedUser, new MongoDBOperationEventArgs(), "InpostClient");
        System.Threading.Thread.Sleep(3500);
    }

    public static void UpdateClient(MongoDBService mongo)
    {
        Inputs.ShowHeader("=== UPDATE CLIENT ===");
        UpdateUserOperation updateUser = new UpdateUserOperation();
        updateUser.Notify += EventListener.OnUserOperation;
        PersonModel updatedUser = new PersonModel(Inputs.GetInputString("Enter username:"), Inputs.GetPassword("Enter new password:"));
        updateUser.Operation(mongo, updatedUser, new MongoDBOperationEventArgs(), "InpostClient");
        System.Threading.Thread.Sleep(3500);
    }

    public static void SearchClient(MongoDBService mongo)
    {
        Inputs.ShowHeader("=== SEARCH CLIENT ===");
        ShowUserOperation showClient = new ShowUserOperation();
        showClient.Notify += EventListener.OnUserOperation;
        PersonModel searchedDeliverer = new PersonModel(Inputs.GetInputString("Enter username: "), "");
        showClient.Operation(mongo, searchedDeliverer, new MongoDBOperationEventArgs(), "InpostClient");
        Console.ReadKey();
    }

    public static void ShowDeliverers(PersonModel loggedIn,MongoDBService mongo){
        Inputs.ShowHeader("=== DELIVERERS ===");
        ShowUsersOperation showUsers = new ShowUsersOperation();
        showUsers.Notify += EventListener.OnUserOperation;
        showUsers.Operation(mongo, loggedIn, new MongoDBOperationEventArgs(), "InpostEmployee");
        Console.ReadKey();
    }

    public static void AddDeliverer(MongoDBService mongo){
        Inputs.ShowHeader("=== ADD DELIVERER ===");
        AddUserOperation addUser = new AddUserOperation();
        addUser.Notify += EventListener.OnUserOperation;
        
        bool roleAdded = false;
        string username = Inputs.GetInputString("Enter username:");
        PersonModel addedUser = new PersonModel(username, "");
        
        foreach (var databaseUser in DatabaseSearch.FindUsers())
        {
            if (username == databaseUser.Key && !databaseUser.Value.Roles.Contains(Role.InpostEmployee))
            {
                addUser.Operation(mongo, addedUser, new MongoDBOperationEventArgs(), "InpostEmployee");
                roleAdded = true;
                break;
            }
        }

        if (!roleAdded)
        {
            addedUser.Password = Inputs.GetPassword("Enter password:");
            addUser.Operation(mongo, addedUser, new MongoDBOperationEventArgs(), "InpostEmployee");
        }
        
        System.Threading.Thread.Sleep(3500);
    }

    public static void DeleteDeliverer(MongoDBService mongo){
        Inputs.ShowHeader("=== DELETE DELIVERER ===");
        DeleteUserOperation deleteUser = new DeleteUserOperation();
        deleteUser.Notify += EventListener.OnUserOperation;
        PersonModel deletedUser = new PersonModel(Inputs.GetInputString("Enter username:"), "");
        deleteUser.Operation(mongo, deletedUser, new MongoDBOperationEventArgs(), "InpostEmployee");
        System.Threading.Thread.Sleep(3500);
    }

    public static void UpdateDeliverer(MongoDBService mongo)
    {
        Inputs.ShowHeader("=== UPDATE DELIVERER ===");
        UpdateUserOperation updateDeliverer = new UpdateUserOperation();
        updateDeliverer.Notify += EventListener.OnUserOperation;
        PersonModel updatedDeliverer = new PersonModel(Inputs.GetInputString("Enter username: "), Inputs.GetPassword("Enter new password: "));
        updateDeliverer.Operation(mongo, updatedDeliverer, new MongoDBOperationEventArgs(), "InpostEmployee");
        System.Threading.Thread.Sleep(3500);
    }

    public static void SearchDeliverer(MongoDBService mongo)
    {
        Inputs.ShowHeader("=== SEARCH DELIVERER ===");
        ShowUserOperation showDeliverer = new ShowUserOperation();
        showDeliverer.Notify += EventListener.OnUserOperation;
        PersonModel searchedDeliverer = new PersonModel(Inputs.GetInputString("Enter username: "), "");
        showDeliverer.Operation(mongo, searchedDeliverer, new MongoDBOperationEventArgs(), "InpostEmployee");
        Console.ReadKey();
    }

    public static void ShowUsers(PersonModel loggedIn, MongoDBService mongo)
    {
        Inputs.ShowHeader("=== USERS ===");
        ShowUsersOperation showUsers = new ShowUsersOperation();
        showUsers.Notify += EventListener.OnUserOperation;
        showUsers.Operation(mongo, loggedIn, new MongoDBOperationEventArgs(), "");
        Console.ReadKey();
    }

    public static void AddUser(MongoDBService mongo)
    {
        Inputs.ShowHeader("=== ADD USER ===");
        AddUserOperation addUser = new AddUserOperation();
        addUser.Notify += EventListener.OnUserOperation;
        
        bool roleAdded = false;
        string username = Inputs.GetInputString("Enter username:");
        PersonModel addedUser = new PersonModel(username, "");
        
        Role r;
        while(!Enum.TryParse(Inputs.GetInputString("Enter role:"), out r))
        {
            Inputs.ShowError("Invalid role. Please enter one of following roles:");
            Console.WriteLine("-Administrator");
            Console.WriteLine("-InpostClient");
            Console.WriteLine("-InpostEmployee");
        }
        
        foreach (var databaseUser in DatabaseSearch.FindUsers())
        {
            if (username == databaseUser.Key && !databaseUser.Value.Roles.Contains(r))
            {
                addUser.Operation(mongo, addedUser, new MongoDBOperationEventArgs(), r.ToString());
                roleAdded = true;
                break;
            }
        }

        if (!roleAdded)
        {
            addedUser.Password = Inputs.GetPassword("Enter password:");
            addUser.Operation(mongo, addedUser, new MongoDBOperationEventArgs(), r.ToString());
        }
        
        System.Threading.Thread.Sleep(3500);
    }

    public static void DeleteUser(MongoDBService mongo)
    {
        Inputs.ShowHeader("=== DELETE USER ===");
        DeleteUserOperation deleteUser = new DeleteUserOperation();
        deleteUser.Notify += EventListener.OnUserOperation;
        PersonModel deletedUser = new PersonModel(Inputs.GetInputString("Enter username:"), "");
        deleteUser.Operation(mongo, deletedUser, new MongoDBOperationEventArgs(), "");
        System.Threading.Thread.Sleep(3500);
    }

    public static void UpdateUser(MongoDBService mongo)
    {
        Inputs.ShowHeader("=== UPDATE USER ===");
        UpdateUserOperation updateUser = new UpdateUserOperation();
        updateUser.Notify += EventListener.OnUserOperation;
        PersonModel updatedUser = new PersonModel(Inputs.GetInputString("Enter username:"), Inputs.GetPassword("Enter new password:"));
        updateUser.Operation(mongo, updatedUser, new MongoDBOperationEventArgs(), "");
        System.Threading.Thread.Sleep(3500);
    }

    public static void SearchUser(MongoDBService mongo)
    {
        Inputs.ShowHeader("=== SEARCH USER ===");
        ShowUserOperation showUser = new ShowUserOperation();
        showUser.Notify += EventListener.OnUserOperation;
        PersonModel searchedUser = new PersonModel(Inputs.GetInputString("Enter username: "), "");
        showUser.Operation(mongo, searchedUser, new MongoDBOperationEventArgs(), "");
        Console.ReadKey();
    }

    public static void ShowPackages(PersonModel loggedIn, MongoDBService mongo)
    {
        Inputs.ShowHeader("=== PACKAGES ===");
        ShowParcelsOperation showParcels = new ShowParcelsOperation();
        showParcels.Notify += EventListener.OnUserOperation;
        showParcels.Operation(mongo, loggedIn, new MongoDBOperationEventArgs(), "InpostEmployeeAll");
        Console.ReadKey();
    }

    public static void ShowMyPackages(PersonModel loggedIn, MongoDBService mongo)
    {
        Inputs.ShowHeader("=== MY PACKAGES ===");
        ShowParcelsOperation showParcels = new ShowParcelsOperation();
        showParcels.Notify += EventListener.OnUserOperation;
        showParcels.Operation(mongo, loggedIn, new MongoDBOperationEventArgs(), "");
        Console.ReadKey();
    }

    public static void AddPackage(MongoDBService mongo)
    {
        Inputs.ShowHeader("=== ADD PACKAGE ===");
        AddParcelOperation addParcel = new AddParcelOperation();
        addParcel.Notify += EventListener.OnParcelOperation;
        PersonModel personModel = new PersonModel(Inputs.GetInputString("Enter recipient username:"), "");
        ParcelModel addedParcel = new ParcelModel(Inputs.GetInputString("Enter parcel name:"), personModel);
        addParcel.Operation(mongo, addedParcel, personModel ,new MongoDBOperationEventArgs());
        System.Threading.Thread.Sleep(3500);
    }

    public static void OrderPackage(PersonModel loggedIn, MongoDBService mongo)
    {
        Inputs.ShowHeader("=== ORDER PACKAGE ===");
        AddParcelOperation addParcel = new AddParcelOperation();
        addParcel.Notify += EventListener.OnParcelOperation;
        ParcelModel addedParcel = new ParcelModel(Inputs.GetInputString("Enter parcel name:"), loggedIn);
        addParcel.Operation(mongo, addedParcel, loggedIn, new MongoDBOperationEventArgs());
        System.Threading.Thread.Sleep(3500);
    }

    public static void DeletePackage(PersonModel loggedIn, MongoDBService mongo)
    {
        Inputs.ShowHeader("=== DELETE PACKAGE ===");
        DeleteParcelOperation deleteParcel = new DeleteParcelOperation();
        deleteParcel.Notify += EventListener.OnParcelOperation;
        ParcelModel deletedParcel = new ParcelModel(Inputs.GetInputString("Enter parcel name:"), loggedIn);
        deleteParcel.Operation(mongo, deletedParcel, loggedIn, new MongoDBOperationEventArgs());
        System.Threading.Thread.Sleep(3500);
    }

    public static void UpdatePackage(PersonModel loggedIn, MongoDBService mongo)
    {
        Inputs.ShowHeader("=== UPDATE PACKAGE ===");
        UpdateParcelOperation updateParcel = new UpdateParcelOperation();
        updateParcel.Notify += EventListener.OnParcelOperation;
        ParcelModel updatedParcel = new ParcelModel(Inputs.GetInputString("Enter parcel name:"), null);
        PersonModel delivererModel = new PersonModel(Inputs.GetInputString("Enter new deliverer username:"), "");
        updatedParcel.ChangeSender(delivererModel);
        ParcelStatus s;
        while (!Enum.TryParse(Inputs.GetInputString("Enter new status:"), out s))
        {
            Console.WriteLine("Invalid status. Please enter one of following statuses:");
            Console.WriteLine("-InWarehouse");
            Console.WriteLine("-InTransport");
            Console.WriteLine("-Delivered");
        }
        updatedParcel.ChangeStatus(s);
        updateParcel.Operation(mongo, updatedParcel, loggedIn, new MongoDBOperationEventArgs());
        System.Threading.Thread.Sleep(3500);
    }

    public static void PickUpPackage(PersonModel loggedIn, MongoDBService mongo)
    {
        Inputs.ShowHeader("=== PICK UP PACKAGE ===");
        UpdateParcelOperation updateParcel = new UpdateParcelOperation();
        updateParcel.Notify += EventListener.OnParcelOperation;
        ParcelModel updatedParcel = new ParcelModel(Inputs.GetInputString("Enter parcel name:"), new PersonModel("", ""));
        updatedParcel.ChangeStatus(ParcelStatus.InTransport);
        updatedParcel.ChangeSender(loggedIn);
        updateParcel.Operation(mongo, updatedParcel, loggedIn, new MongoDBOperationEventArgs());
        System.Threading.Thread.Sleep(3500);
    }

    public static void DeliverPackage(PersonModel loggedIn, MongoDBService mongo)
    {
        Inputs.ShowHeader("=== DELIVER PACKAGE ===");
        UpdateParcelOperation updateParcel = new UpdateParcelOperation();
        updateParcel.Notify += EventListener.OnParcelOperation;
        ParcelModel updatedParcel = new ParcelModel(Inputs.GetInputString("Enter parcel name:"), new PersonModel("", ""));
        updatedParcel.ChangeStatus(ParcelStatus.Delivered);
        updatedParcel.ChangeSender(loggedIn);
        updateParcel.Operation(mongo, updatedParcel, loggedIn, new MongoDBOperationEventArgs());
        System.Threading.Thread.Sleep(3500);
    }

    public static void SearchPackage(PersonModel loggedIn, MongoDBService mongo)
    {
        Inputs.ShowHeader("=== SEARCH PACKAGE ===");
        ShowParcelOperation showParcel = new ShowParcelOperation();
        showParcel.Notify += EventListener.OnParcelOperation;
        ParcelModel searchedParcel = new ParcelModel(Inputs.GetInputString("Enter parcel name:"), new PersonModel("", ""));
        showParcel.Operation(mongo, searchedParcel, loggedIn, new MongoDBOperationEventArgs());
        Console.ReadKey();
    }
}
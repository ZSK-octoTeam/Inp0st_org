using System.Text;
using Inpost_org.Services.Operations.ParcelOperations;
using Inpost_org.Services.Operations.UserOperations;
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Services.Operations;
using Inpost_org.Services;
using Inpost_org.Tests;
using Inpost_org.Users.Deliveries;
using Inpost_org.Users;
using MongoDB.Driver;
using MongoDB.Bson;

internal class Program
{
    public static string GetInputString(string prompt)
    {
        string input;
        Console.WriteLine(prompt);
        input = Console.ReadLine();
        while (String.IsNullOrEmpty(input))
        {
            Console.WriteLine("Invalid input. Please enter a correct string:");
            input = Console.ReadLine();
        }
        return input;
    }

    public static string GetPassword(string prompt)
    {
        var password = new StringBuilder();
        ConsoleKeyInfo key;
        Console.WriteLine(prompt);
        while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password.Length -= 1;
                    Console.Write("\b \b");
                }
            }
            else
            {
                password.Append(key.KeyChar);
                Console.Write("*");
            }
        }

        Console.WriteLine();
        return password.ToString();
    }
    
    public static int GetInputInt(string prompt)
    {
        int input;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(prompt);
        Console.ResetColor();
        while (!int.TryParse(Console.ReadLine(), out input))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input. Please enter a correct number:");
            Console.ResetColor();
        }
        return input;
    }

    public static void ShowHeader(string prompt)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(prompt);
        Console.ResetColor();
    }

    public static void ShowError(string prompt)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(prompt);
        Console.ResetColor();
    }

    public static bool CheckCredentials(string username, string password)
    {
        if(DatabaseSearch.FindUsers().ContainsKey(username))
        {
            DatabaseSearch.FindUsers().TryGetValue(username, out PersonModel databasePerson);
                
            if (databasePerson.Password == DatabaseSearch.HashPassword(password))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Log in successful.");
                Console.ResetColor();
                System.Threading.Thread.Sleep(2500);
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Log in failed. Wrong password.");
                Console.ResetColor();
                    
                System.Threading.Thread.Sleep(2500);
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Log in failed. User not found.");
            Console.ResetColor();
            System.Threading.Thread.Sleep(2500);
        }

        return false;
    }
    
    public static PersonModel LogIn()
    {
        string username = "";
        string password = "";
        
        do
        {
            ShowHeader("=== LOG IN ===");
            username = GetInputString("Enter your username:");
            password = GetPassword("Enter your password:");
        }while(!CheckCredentials(username, password));
        
        DatabaseSearch.FindUsers().TryGetValue(username, out PersonModel loggedIn);
        return loggedIn;
    }
    
    public static void ShowMenu(PersonModel loggedIn, MongoDBService mongo)
    {
        while(loggedIn.Roles.Contains(Role.Administrator))
        {
            ShowHeader("=== MENU ===");
            Console.WriteLine("1. Menage clients");
            Console.WriteLine("2. Menage deliverers");
            Console.WriteLine("3. Menage packages");
            Console.WriteLine("4. Menage users");
            Console.WriteLine("5. Log out");
            Console.WriteLine("6. Exit");
            
            int choice = GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    ShowClientsMenu(loggedIn, mongo);
                    break;
                case 2:
                    ShowDeliverersMenu(loggedIn, mongo);
                    break;
                case 3:
                    ShowPackagesMenu(loggedIn, mongo);
                    break;
                case 4:
                    ShowUsersMenu(loggedIn, mongo);
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine($"Logged out successfully.");
                    loggedIn = LogIn();
                    ChooseMenu(loggedIn, mongo);
                    break;
                case 6:
                    Environment.Exit(0);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a correct number:");
                    Console.ResetColor();
                    System.Threading.Thread.Sleep(2500);
                    break;
            }
        }
    }

    public static void ShowClientsMenu(PersonModel loggedIn, MongoDBService mongo)
    {
        while(true){
            ShowHeader("=== CLIENTS MENU ===");
            Console.WriteLine("1. Add client");
            Console.WriteLine("2. Show all clients");
            Console.WriteLine("3. Delete client");
            Console.WriteLine("4. Update client");
            Console.WriteLine("5. Search client");
            Console.WriteLine("6. Back");
            Console.WriteLine("7. Exit");
            
            int choice = GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    AddClient(mongo);
                    break;
                case 2:
                    ShowClients(loggedIn, mongo);
                    break;
                case 3:
                    DeleteClient(mongo);
                    break;
                case 4:
                    UpdateClient(mongo);
                    break;
                case 5:
                    SearchClient(mongo);
                    break;
                case 6:
                    ShowMenu(loggedIn, mongo);
                    break;
                case 7:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter a correct number:");
                    break;
            }
        }
    }

    public static void ShowDeliverersMenu(PersonModel loggedIn, MongoDBService mongo)
    {
        while(true){
            ShowHeader("=== DELIVERERS MENU ===");
            Console.WriteLine("1. Add deliverer");
            Console.WriteLine("2. Show all deliverers");
            Console.WriteLine("3. Delete deliverer");
            Console.WriteLine("4. Update deliverer");
            Console.WriteLine("5. Search deliverer");
            Console.WriteLine("6. Back");
            Console.WriteLine("7. Exit");
            
            int choice = GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    AddDeliverer(mongo);
                    break;
                case 2:
                    ShowDeliverers(loggedIn, mongo);
                    break;
                case 3:
                    DeleteDeliverer(mongo);
                    break;
                case 4:
                    UpdateDeliverer(mongo);
                    break;
                case 5:
                    SearchDeliverer(mongo);
                    break;
                case 6:
                    ShowMenu(loggedIn, mongo);
                    break;
                case 7:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    public static void ShowPackagesMenu(PersonModel loggedIn, MongoDBService mongo)
    {
        while(true){
            ShowHeader("=== PACKAGES MENU ===");
            Console.WriteLine("1. Add package");
            Console.WriteLine("2. Show all packages");
            Console.WriteLine("3. Delete package");
            Console.WriteLine("4. Update package");
            Console.WriteLine("5. Search package");
            Console.WriteLine("6. Back");
            Console.WriteLine("7. Exit");
            
            int choice = GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    AddPackage(mongo);
                    break;
                case 2:
                    ShowPackages(loggedIn, mongo);
                    break;
                case 3:
                    DeletePackage(loggedIn, mongo);
                    break;
                case 4:
                    UpdatePackage(loggedIn ,mongo);
                    break;
                case 5:
                    SearchPackage(loggedIn, mongo);
                    break;
                case 6:
                    ShowMenu(loggedIn, mongo);
                    break;
                case 7:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter a correct number:");
                    break;
            }
        }
    }

    public static void ShowNormalMenu(PersonModel loggedIn, MongoDBService mongo)
    {
        while(!loggedIn.Roles.Contains(Role.Administrator)){
            ShowHeader("=== MENU ===");
            Console.WriteLine("1. Menage packages");
            Console.WriteLine("2. Log out");
            Console.WriteLine("3. Exit");
            
            int choice = GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    if(loggedIn.Roles.Contains(Role.InpostClient)){
                        ShowPackagesMenuClient(loggedIn, mongo);
                    }
                    else{
                        ShowPackagesMenuDelivery(loggedIn, mongo);
                    }
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine($"Logged out successfully.");
                    loggedIn = LogIn();
                    ChooseMenu(loggedIn, mongo);
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter a correct number:");
                    break;
            }
        }
    }

    public static void ShowCADMenu(PersonModel loggedIn, MongoDBService mongo)
    {
        while (loggedIn.Roles.Contains(Role.InpostEmployee) && loggedIn.Roles.Contains(Role.InpostClient))
        {
            ShowHeader("=== Customer and Deliverer Menu ===");
            Console.WriteLine("1. Menage packages(deliverer)");
            Console.WriteLine("2. Menage packages(client)");
            Console.WriteLine("3. Log out");
            Console.WriteLine("4. Exit");

            int choice = GetInputInt("Enter your choice:");
            switch (choice)
            {
                case 1:
                    ShowPackagesMenuDelivery(loggedIn, mongo);
                    break;
                case 2:
                    ShowPackagesMenuClient(loggedIn, mongo);
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine($"Logged out successfully.");
                    loggedIn = LogIn();
                    ChooseMenu(loggedIn, mongo);
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter a correct number:");
                    break;
            }
        }
    }

    public static void ChooseMenu(PersonModel loggedIn, MongoDBService mongo)
    {
        if (loggedIn.Roles.Contains(Role.Administrator))
        {
            ShowMenu(loggedIn, mongo);
        }
        else if (loggedIn.Roles.Contains(Role.InpostClient) && loggedIn.Roles.Contains(Role.InpostEmployee))
        {
            ShowCADMenu(loggedIn, mongo);
        }
        else
        {
            ShowNormalMenu(loggedIn, mongo);
        }
    }

    public static void ShowUsersMenu(PersonModel loggedIn, MongoDBService mongo)
    {
        while(true){
            ShowHeader("=== USERS MENU ===");
            Console.WriteLine("1. Add user");
            Console.WriteLine("2. Show all users");
            Console.WriteLine("3. Update user");
            Console.WriteLine("4. Delete user");
            Console.WriteLine("5. Search user");
            Console.WriteLine("6. Back");
            Console.WriteLine("7. Exit");
            
            int choice = GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    AddUser(mongo);
                    break;
                case 2:
                    ShowUsers(loggedIn, mongo);
                    break;
                case 3:
                    UpdateUser(mongo);
                    break;
                case 4:
                    DeleteUser(mongo);
                    break;
                case 5:
                    SearchUser(mongo);
                    break;
                case 6:
                    ShowMenu(loggedIn, mongo);
                    break;
                case 7:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter a correct number:");
                    break;
            }
        }
    }

    public static void ShowPackagesMenuClient(PersonModel loggedIn, MongoDBService mongo)
    {
        while(true){
            ShowHeader("=== PACKAGES MENU ===");
            Console.WriteLine("1. Order package");
            Console.WriteLine("2. Show all my packages");
            Console.WriteLine("3. Cancel package");
            Console.WriteLine("4. Search package");
            Console.WriteLine("5. Back");
            Console.WriteLine("6. Exit");
            
            int choice = GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    OrderPackage(loggedIn, mongo);
                    break;
                case 2:
                    ShowMyPackages(loggedIn, mongo);
                    break;
                case 3:
                    DeletePackage(loggedIn, mongo);
                    break;
                case 4:
                    SearchPackage(loggedIn, mongo);
                    break;
                case 5:
                    ChooseMenu(loggedIn, mongo);
                    break;
                case 6:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter a correct number:");
                    break;
            }
        }
    }

    public static void ShowPackagesMenuDelivery(PersonModel loggedIn, MongoDBService mongo)
    {
        while(true){
            ShowHeader("=== PACKAGES MENU ===");
            Console.WriteLine("1. Show all my packages");
            Console.WriteLine("2. Show all packages");
            Console.WriteLine("3. Pick up package");
            Console.WriteLine("4. Deliver package");
            Console.WriteLine("5. Search package");
            Console.WriteLine("6. Back");
            Console.WriteLine("7. Exit");
            
            int choice = GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    ShowMyPackages(loggedIn, mongo);
                    break;
                case 2:
                    ShowPackages(loggedIn, mongo);
                    break;
                case 3:
                    PickUpPackage(loggedIn, mongo);
                    break;
                case 4:
                    DeliverPackage(loggedIn, mongo);
                    break;
                case 5:
                    SearchPackage(loggedIn, mongo);
                    break;
                case 6:
                    ChooseMenu(loggedIn, mongo);
                    break;
                case 7:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter a correct number:");
                    break;
            }
        }
    }

    public static void ShowClients(PersonModel loggedIn,MongoDBService mongo){
        ShowHeader("=== CLIENTS ===");
        ShowUsersOperation showUsers = new ShowUsersOperation();
        showUsers.Notify += EventListener.OnUserOperation;
        showUsers.Operation(mongo, loggedIn, new MongoDBOperationEventArgs(), "InpostClient");
        Console.ReadKey();
    }

    public static void AddClient(MongoDBService mongo)
    {
        ShowHeader("=== ADD CLIENT ===");
        AddUserOperation addUser = new AddUserOperation();
        addUser.Notify += EventListener.OnUserOperation;
        PersonModel addedUser = new PersonModel(GetInputString("Enter username:"), GetPassword("Enter password:"));
        Console.WriteLine();
        addUser.Operation(mongo, addedUser, new MongoDBOperationEventArgs(), "InpostClient");
        System.Threading.Thread.Sleep(3500);
    }

    public static void DeleteClient(MongoDBService mongo){
        ShowHeader("=== DELETE CLIENT ===");
        DeleteUserOperation deleteUser = new DeleteUserOperation();
        deleteUser.Notify += EventListener.OnUserOperation;
        PersonModel deletedUser = new PersonModel(GetInputString("Enter username:"), "");
        deleteUser.Operation(mongo, deletedUser, new MongoDBOperationEventArgs(), "InpostClient");
        System.Threading.Thread.Sleep(3500);
    }

    public static void UpdateClient(MongoDBService mongo)
    {
        ShowHeader("=== UPDATE CLIENT ===");
        UpdateUserOperation updateUser = new UpdateUserOperation();
        updateUser.Notify += EventListener.OnUserOperation;
        PersonModel updatedUser = new PersonModel(GetInputString("Enter username:"), GetInputString("Enter new password:"));
        updateUser.Operation(mongo, updatedUser, new MongoDBOperationEventArgs(), "InpostClient");
        System.Threading.Thread.Sleep(3500);
    }

    public static void SearchClient(MongoDBService mongo)
    {
        ShowHeader("=== SEARCH CLIENT ===");
        ShowUserOperation showClient = new ShowUserOperation();
        showClient.Notify += EventListener.OnUserOperation;
        PersonModel searchedDeliverer = new PersonModel(GetInputString("Enter username: "), "");
        showClient.Operation(mongo, searchedDeliverer, new MongoDBOperationEventArgs(), "InpostClient");
        Console.ReadKey();
    }

    public static void ShowDeliverers(PersonModel loggedIn,MongoDBService mongo){
        ShowHeader("=== DELIVERERS ===");
        ShowUsersOperation showUsers = new ShowUsersOperation();
        showUsers.Notify += EventListener.OnUserOperation;
        showUsers.Operation(mongo, loggedIn, new MongoDBOperationEventArgs(), "InpostEmployee");
    }

    public static void AddDeliverer(MongoDBService mongo){
        ShowHeader("=== ADD DELIVERER ===");
        AddUserOperation addUser = new AddUserOperation();
        addUser.Notify += EventListener.OnUserOperation;
        PersonModel addedUser = new PersonModel(GetInputString("Enter username:"), GetInputString("Enter password:"));
        addUser.Operation(mongo, addedUser, new MongoDBOperationEventArgs(), "InpostEmployee");
    }

    public static void DeleteDeliverer(MongoDBService mongo){
        ShowHeader("=== DELETE DELIVERER ===");
        DeleteUserOperation deleteUser = new DeleteUserOperation();
        deleteUser.Notify += EventListener.OnUserOperation;
        PersonModel deletedUser = new PersonModel(GetInputString("Enter username:"), "");
        deleteUser.Operation(mongo, deletedUser, new MongoDBOperationEventArgs(), "InpostEmployee");
    }

    public static void UpdateDeliverer(MongoDBService mongo)
    {
        ShowHeader("=== UPDATE DELIVERER ===");
        UpdateUserOperation updateDeliverer = new UpdateUserOperation();
        updateDeliverer.Notify += EventListener.OnUserOperation;
        PersonModel updatedDeliverer = new PersonModel(GetInputString("Enter username: "), GetInputString("Enter new password: "));
        updateDeliverer.Operation(mongo, updatedDeliverer, new MongoDBOperationEventArgs(), "InpostEmployee");
    }

    public static void SearchDeliverer(MongoDBService mongo)
    {
        ShowHeader("=== SEARCH DELIVERER ===");
        ShowUserOperation showDeliverer = new ShowUserOperation();
        showDeliverer.Notify += EventListener.OnUserOperation;
        PersonModel searchedDeliverer = new PersonModel(GetInputString("Enter username: "), "");
        showDeliverer.Operation(mongo, searchedDeliverer, new MongoDBOperationEventArgs(), "InpostEmployee");
    }

    public static void ShowUsers(PersonModel loggedIn, MongoDBService mongo)
    {
        ShowHeader("=== USERS ===");
        ShowUsersOperation showUsers = new ShowUsersOperation();
        showUsers.Notify += EventListener.OnUserOperation;
        showUsers.Operation(mongo, loggedIn, new MongoDBOperationEventArgs(), "");
    }

    public static void AddUser(MongoDBService mongo)
    {
        ShowHeader("=== ADD USER ===");
        AddUserOperation addUser = new AddUserOperation();
        addUser.Notify += EventListener.OnUserOperation;
        PersonModel addedUser = new PersonModel(GetInputString("Enter username:"), GetInputString("Enter password:"));
        Role r;
        while(!Enum.TryParse(GetInputString("Enter role:"), out r))
        {
            ShowError("Invalid role. Please enter one of following roles:");
            Console.WriteLine("-Administrator");
            Console.WriteLine("-InpostClient");
            Console.WriteLine("-InpostEmployee");
        }
        addedUser.Roles.Add(r);
        addUser.Operation(mongo, addedUser, new MongoDBOperationEventArgs(), "");
        System.Threading.Thread.Sleep(2500);
    }

    public static void DeleteUser(MongoDBService mongo)
    {
        ShowHeader("=== DELETE USER ===");
        DeleteUserOperation deleteUser = new DeleteUserOperation();
        deleteUser.Notify += EventListener.OnUserOperation;
        PersonModel deletedUser = new PersonModel(GetInputString("Enter username:"), "");
        deleteUser.Operation(mongo, deletedUser, new MongoDBOperationEventArgs(), "");
    }

    public static void UpdateUser(MongoDBService mongo)
    {
        ShowHeader("=== UPDATE USER ===");
        UpdateUserOperation updateUser = new UpdateUserOperation();
        updateUser.Notify += EventListener.OnUserOperation;
        PersonModel updatedUser = new PersonModel(GetInputString("Enter username:"), GetInputString("Enter new password:"));
        updateUser.Operation(mongo, updatedUser, new MongoDBOperationEventArgs(), "");
    }

    public static void SearchUser(MongoDBService mongo)
    {
        ShowHeader("=== SEARCH USER ===");
        ShowUserOperation showUser = new ShowUserOperation();
        showUser.Notify += EventListener.OnUserOperation;
        PersonModel searchedUser = new PersonModel(GetInputString("Enter username: "), "");
        showUser.Operation(mongo, searchedUser, new MongoDBOperationEventArgs(), "");
    }

    public static void ShowPackages(PersonModel loggedIn, MongoDBService mongo)
    {
        ShowHeader("=== PACKAGES ===");
        ShowParcelsOperation showParcels = new ShowParcelsOperation();
        showParcels.Notify += EventListener.OnUserOperation;
        showParcels.Operation(mongo, loggedIn, new MongoDBOperationEventArgs(), "InpostEmployeeAll");
        Console.ReadKey();
    }

    public static void ShowMyPackages(PersonModel loggedIn, MongoDBService mongo)
    {
        ShowHeader("=== MY PACKAGES ===");
        ShowParcelsOperation showParcels = new ShowParcelsOperation();
        showParcels.Notify += EventListener.OnUserOperation;
        showParcels.Operation(mongo, loggedIn, new MongoDBOperationEventArgs(), "");
        Console.ReadKey();
    }

    public static void AddPackage(MongoDBService mongo)
    {
        ShowHeader("=== ADD PACKAGE ===");
        AddParcelOperation addParcel = new AddParcelOperation();
        addParcel.Notify += EventListener.OnParcelOperation;
        PersonModel personModel = new PersonModel(GetInputString("Enter sender username:"), "");
        ParcelModel addedParcel = new ParcelModel(GetInputString("Enter parcel name:"), personModel);
        addParcel.Operation(mongo, addedParcel, personModel ,new MongoDBOperationEventArgs());
    }

    public static void OrderPackage(PersonModel loggedIn, MongoDBService mongo)
    {
        ShowHeader("=== ORDER PACKAGE ===");
        AddParcelOperation addParcel = new AddParcelOperation();
        addParcel.Notify += EventListener.OnParcelOperation;
        ParcelModel addedParcel = new ParcelModel(GetInputString("Enter parcel name:"), loggedIn);
        addParcel.Operation(mongo, addedParcel, loggedIn, new MongoDBOperationEventArgs());
        System.Threading.Thread.Sleep(3500);
    }

    public static void DeletePackage(PersonModel loggedIn, MongoDBService mongo)
    {
        ShowHeader("=== DELETE PACKAGE ===");
        DeleteParcelOperation deleteParcel = new DeleteParcelOperation();
        deleteParcel.Notify += EventListener.OnParcelOperation;
        ParcelModel deletedParcel = new ParcelModel(GetInputString("Enter parcel name:"), loggedIn);
        deleteParcel.Operation(mongo, deletedParcel, loggedIn, new MongoDBOperationEventArgs());
    }

    public static void UpdatePackage(PersonModel loggedIn, MongoDBService mongo)
    {
        ShowHeader("=== UPDATE PACKAGE ===");
        UpdateParcelOperation updateParcel = new UpdateParcelOperation();
        updateParcel.Notify += EventListener.OnParcelOperation;
        ParcelModel updatedParcel = new ParcelModel(GetInputString("Enter parcel name:"), new PersonModel(GetInputString("Enter new recipient username:"), ""));
        PersonModel delivererModel = new PersonModel(GetInputString("Enter new deliverer username:"), "");
        updatedParcel.ChangeSender(delivererModel);
        ParcelStatus s;
        while (!Enum.TryParse(GetInputString("Enter new status:"), out s))
        {
            Console.WriteLine("Invalid status. Please enter one of following statuses:");
            Console.WriteLine("-InWarehouse");
            Console.WriteLine("-InTransport");
            Console.WriteLine("-Delivered");
        }
        updatedParcel.ChangeStatus(s);
        updateParcel.Operation(mongo, updatedParcel, loggedIn, new MongoDBOperationEventArgs());
    }

    public static void PickUpPackage(PersonModel loggedIn, MongoDBService mongo)
    {
        ShowHeader("=== PICK UP PACKAGE ===");
        UpdateParcelOperation updateParcel = new UpdateParcelOperation();
        updateParcel.Notify += EventListener.OnParcelOperation;
        ParcelModel updatedParcel = new ParcelModel(GetInputString("Enter parcel name:"), new PersonModel("", ""));
        updatedParcel.ChangeStatus(ParcelStatus.InTransport);
        updatedParcel.ChangeSender(loggedIn);
        updateParcel.Operation(mongo, updatedParcel, loggedIn, new MongoDBOperationEventArgs());
    }

    public static void DeliverPackage(PersonModel loggedIn, MongoDBService mongo)
    {
        ShowHeader("=== DELIVER PACKAGE ===");
        UpdateParcelOperation updateParcel = new UpdateParcelOperation();
        updateParcel.Notify += EventListener.OnParcelOperation;
        ParcelModel updatedParcel = new ParcelModel(GetInputString("Enter parcel name:"), new PersonModel("", ""));
        updatedParcel.ChangeStatus(ParcelStatus.Delivered);
        updatedParcel.ChangeSender(loggedIn);
        updateParcel.Operation(mongo, updatedParcel, loggedIn, new MongoDBOperationEventArgs());
    }

    public static void SearchPackage(PersonModel loggedIn, MongoDBService mongo)
    {
        ShowHeader("=== SEARCH PACKAGE ===");
        ShowParcelOperation showParcel = new ShowParcelOperation();
        showParcel.Notify += EventListener.OnParcelOperation;
        ParcelModel searchedParcel = new ParcelModel(GetInputString("Enter parcel name:"), new PersonModel("", ""));
        showParcel.Operation(mongo, searchedParcel, loggedIn, new MongoDBOperationEventArgs());
    }
    public static void Main(string[] args)
    {
        // Database
        MongoDBService mongo = new MongoDBService();
        mongo.Connect();
        DatabaseSearch.mongo = mongo;
        
        //Tests.TestAddUserOperation(mongo);

        AddUserOperation add = new AddUserOperation();
        add.Notify += EventListener.OnUserOperation;
        
        PersonModel loggedIn = LogIn();
        ChooseMenu(loggedIn, mongo);
    }
}
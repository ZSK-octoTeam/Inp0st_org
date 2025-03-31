using Inpost_org.Services.Operations.ParcelOperations;
using Inpost_org.Services.Operations.UserOperations;
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Services.Operations;
using Inpost_org.Services;
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
    
    public static int GetInputInt(string prompt)
    {
        int input;
        Console.WriteLine(prompt);
        while (!int.TryParse(Console.ReadLine(), out input))
        {
            Console.WriteLine("Invalid input. Please enter a correct number:");
        }
        return input;
    }
    
    public static PersonModel LogIn()
    {
        string username = GetInputString("Enter your username:");
        string password = GetInputString("Enter your password:");
    
        PersonModel person = new PersonModel(username, password);
        
        foreach (var databasePerson in DatabaseSearch.FindUsers())
        {
            if (databasePerson.Key == person.Username)
            {
                if(DatabaseSearch.HashPassword(person.Password) == databasePerson.Value.Password)
                {
                    Console.WriteLine("Log in successful.");
                    return person;
                }
                else
                {
                    Console.WriteLine("Log in failed. Wrong password.");
                    LogIn();
                }
            
            }
        }
        Console.WriteLine("Log in failed. User not found.");
        LogIn();
        return null;
    }
    
    public static void ShowMenu(PersonModel loggedIn, MongoDBService mongo)
    {
        while(loggedIn.Roles.Contains(Role.Administrator)){
            Console.WriteLine("=== MENU ===");
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
                    LogIn();
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

    public static void ShowClientsMenu(PersonModel loggedIn, MongoDBService mongo)
    {
        while(true){
            Console.WriteLine("=== CLIENTS MENU ===");
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
            Console.WriteLine("=== DELIVERERS MENU ===");
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
            Console.WriteLine("=== PACKAGES MENU ===");
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
                    //AddPackage();
                    break;
                case 2:
                    //ShowPackage();
                    break;
                case 3:
                    //DeletePackage();
                    break;
                case 4:
                    //UpdatePackage();
                    break;
                case 5:
                    //SearchPackage();
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
            Console.WriteLine("=== MENU ===");
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
                    ShowMenu(loggedIn, mongo);
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

    public static void ShowUsersMenu(PersonModel loggedIn, MongoDBService mongo)
    {
        while(true){
            Console.WriteLine("=== USERS MENU ===");
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
                    //AddUser();
                    break;
                case 2:
                    //ShowUser();
                    break;
                case 3:
                    //UpdateUser();
                    break;
                case 4:
                    //DeleteUser();
                    break;
                case 5:
                    //SearchUser();
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
            Console.WriteLine("=== PACKAGES MENU ===");
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
                    //AddPackage();
                    break;
                case 2:
                    //ShowMyPackages();
                    break;
                case 3:
                    //DeletePackage();
                    break;
                case 4:
                    //SearchPackage();
                    break;
                case 5:
                    ShowNormalMenu(loggedIn, mongo);
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
            Console.WriteLine("=== PACKAGES MENU ===");
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
                    //ShowMyPackages();
                    break;
                case 2:
                    //ShowAllPackages();
                    break;
                case 3:
                    //PickUpPackage();
                    break;
                case 4:
                    //DeliverPackage();
                    break;
                case 5:
                    //SearchPackage();
                    break;
                case 6:
                    ShowNormalMenu(loggedIn, mongo);
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
        Console.WriteLine("=== CLIENTS ===");
        ShowUsersOperation showUsers = new ShowUsersOperation();
        showUsers.Notify += EventListener.OnUserOperation;
        showUsers.Operation(mongo, loggedIn, new MongoDBOperationEventArgs(), "InpostClient");
    }

    public static void AddClient(MongoDBService mongo)
    {
        Console.WriteLine("=== ADD CLIENT ===");
        AddUserOperation addUser = new AddUserOperation();
        addUser.Notify += EventListener.OnUserOperation;
        PersonModel addedUser = new PersonModel(GetInputString("Enter username:"), GetInputString("Enter password:"));
        addUser.Operation(mongo, addedUser, new MongoDBOperationEventArgs(), "InpostClient");
    }

    public static void DeleteClient(MongoDBService mongo){
        Console.WriteLine("=== DELETE CLIENT ===");
        DeleteUserOperation deleteUser = new DeleteUserOperation();
        deleteUser.Notify += EventListener.OnUserOperation;
        PersonModel deletedUser = new PersonModel(GetInputString("Enter username:"), "");
        deleteUser.Operation(mongo, deletedUser, new MongoDBOperationEventArgs(), "InpostClient");
    }

    public static void UpdateClient(MongoDBService mongo)
    {
        Console.WriteLine("=== UPDATE CLIENT ===");
        UpdateUserOperation updateUser = new UpdateUserOperation();
        updateUser.Notify += EventListener.OnUserOperation;
        PersonModel updatedUser = new PersonModel(GetInputString("Enter username:"), GetInputString("Enter new password:"));
        updateUser.Operation(mongo, updatedUser, new MongoDBOperationEventArgs(), "InpostClient");
    }

    public static void SearchClient(MongoDBService mongo)
    {
        Console.WriteLine("=== SEARCH CLIENT ===");
        ShowUserOperation showClient = new ShowUserOperation();
        showClient.Notify += EventListener.OnUserOperation;
        PersonModel searchedDeliverer = new PersonModel(GetInputString("Enter username: "), "");
        showClient.Operation(mongo, searchedDeliverer, new MongoDBOperationEventArgs(), "InpostClient");
    }

    public static void ShowDeliverers(PersonModel loggedIn,MongoDBService mongo){
        Console.WriteLine("=== DELIVERERS ===");
        ShowUsersOperation showUsers = new ShowUsersOperation();
        showUsers.Notify += EventListener.OnUserOperation;
        showUsers.Operation(mongo, loggedIn, new MongoDBOperationEventArgs(), "InpostEmployee");
    }

    public static void AddDeliverer(MongoDBService mongo){
        Console.WriteLine("=== ADD DELIVERER ===");
        AddUserOperation addUser = new AddUserOperation();
        addUser.Notify += EventListener.OnUserOperation;
        PersonModel addedUser = new PersonModel(GetInputString("Enter username:"), GetInputString("Enter password:"));
        addUser.Operation(mongo, addedUser, new MongoDBOperationEventArgs(), "InpostEmployee");
    }

    public static void DeleteDeliverer(MongoDBService mongo){
        Console.WriteLine("=== DELETE DELIVERER ===");
        DeleteUserOperation deleteUser = new DeleteUserOperation();
        deleteUser.Notify += EventListener.OnUserOperation;
        PersonModel deletedUser = new PersonModel(GetInputString("Enter username:"), "");
        deleteUser.Operation(mongo, deletedUser, new MongoDBOperationEventArgs(), "InpostEmployee");
    }

    public static void UpdateDeliverer(MongoDBService mongo)
    {
        Console.WriteLine("=== UPDATE DELIVERER ===");
        UpdateUserOperation updateDeliverer = new UpdateUserOperation();
        updateDeliverer.Notify += EventListener.OnUserOperation;
        PersonModel updatedDeliverer = new PersonModel(GetInputString("Enter username: "), GetInputString("Enter new password: "));
        updateDeliverer.Operation(mongo, updatedDeliverer, new MongoDBOperationEventArgs(), "InpostEmployee");
    }

    public static void SearchDeliverer(MongoDBService mongo)
    {
        Console.WriteLine("=== SEARCH DELIVERER ===");
        ShowUserOperation showDeliverer = new ShowUserOperation();
        showDeliverer.Notify += EventListener.OnUserOperation;
        PersonModel searchedDeliverer = new PersonModel(GetInputString("Enter username: "), "");
        showDeliverer.Operation(mongo, searchedDeliverer, new MongoDBOperationEventArgs(), "InpostEmployee");
    }
    
    public static void Main(string[] args)
    {
        // Database
        MongoDBService mongo = new MongoDBService();
        mongo.Connect();
        DatabaseSearch.mongo = mongo;

        UserBase add = new AddUserOperation();
        add.Notify += EventListener.OnUserOperation;
        
        // Log in and show menu
        PersonModel loggedIn = LogIn();
        ShowMenu(loggedIn, mongo);
    }
}
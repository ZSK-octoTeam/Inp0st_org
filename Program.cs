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
    
    public static PersonModel LogIn(MongoDBService mongo)
    {
            string username = GetInputString("Enter your username:");
            string password = GetInputString("Enter your password:");
        
            PersonModel person = new PersonModel(username, password);
        
            foreach (var databasePerson in mongo.collectionUsers.Find(new BsonDocument()).ToList())
            {
                if (databasePerson.Username == person.Username)
                {
                    if(DatabaseSearch.HashPassword(person.Password) == databasePerson.Password)
                    {
                        person = databasePerson;
                        Console.WriteLine("Log in successful.");
                        return person;
                    }
                    else
                    {
                        Console.WriteLine("Log in failed. Wrong password.");
                        LogIn(mongo);
                        return null;
                    }
                
                }
            }
            Console.WriteLine("Log in failed. User not found.");
            LogIn(mongo);
            return null;
    }
    
    public static MongoDBService ConnectToDatabase()
    {
        string username = GetInputString("Enter database user:");
        string passphrase = GetInputString("Enter database user passphrase:");
        
        MongoDBService mongo = new MongoDBService(username, passphrase);
        
        while (!mongo.Connect())
        {
            Console.WriteLine("Connection failed. Try again.");
            username = GetInputString("Enter database user:");
            passphrase = GetInputString("Enter database user passphrase:");
            mongo.SetUser(username, passphrase);
        }
        
        Console.WriteLine("Connection successful.");
        return mongo;
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
                    loggedIn = LogIn(mongo);
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
                    //UpdateClient();
                    break;
                case 5:
                    //SearchClient();
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
                    //UpdateDeliverer();
                    break;
                case 5:
                    //SearchDeliverer();
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
                    loggedIn = LogIn(mongo);
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
        showUsers.Operation(mongo, loggedIn, new MongoDBOperationEventArgs());
    }

    public static void AddClient(MongoDBService mongo){
        Console.WriteLine("=== ADD CLIENT ===");
        AddUserOperation addUser = new AddUserOperation();
        addUser.Notify += EventListener.OnUserOperation;
        PersonModel addedUser = new PersonModel(GetInputString("Enter username:"), GetInputString("Enter password:"));
        foreach (var databasePerson in mongo.collectionUsers.Find(new BsonDocument()).ToList())
        {
            if (databasePerson.Username == addedUser.Username)
            {
                if(!databasePerson.Roles.Contains(Role.InpostClient))
                {
                    databasePerson.AddRole(Role.InpostClient);
                    var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, databasePerson.Username);
                    var update = Builders<PersonModel>.Update.Set(r => r.Roles, databasePerson.Roles);
                    mongo.collectionUsers.UpdateOne(filter, update);
                    Console.WriteLine("Added role client to user.");
                    return;
                }
            }
        }
        addedUser.AddRole(Role.InpostClient);
        addUser.Operation(mongo, addedUser, new MongoDBOperationEventArgs());
    }

    public static void DeleteClient(MongoDBService mongo){
        Console.WriteLine("=== DELETE CLIENT ===");
        DeleteUserOperation deleteUser = new DeleteUserOperation();
        deleteUser.Notify += EventListener.OnUserOperation;
        PersonModel deletedUser = new PersonModel(GetInputString("Enter username:"), "");
        foreach (var databasePerson in mongo.collectionUsers.Find(new BsonDocument()).ToList())
        {
            if (databasePerson.Username == deletedUser.Username)
            {
                if(databasePerson.Roles.Contains(Role.InpostClient))
                {
                    databasePerson.Roles.Remove(Role.InpostClient);
                }
                else
                {
                    Console.WriteLine("User is not a client.");
                    return;
                }

                if(databasePerson.Roles.Count != 0)
                {
                    var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, databasePerson.Username);
                    var update = Builders<PersonModel>.Update.Set(r => r.Roles, databasePerson.Roles);
                    mongo.collectionUsers.UpdateOne(filter, update);
                    System.Console.WriteLine("Deleted role client from user.");
                    return;
                    
                }
            }
        }
        deleteUser.Operation(mongo, deletedUser, new MongoDBOperationEventArgs());
        return;
    }

    public static void ShowDeliverers(PersonModel loggedIn,MongoDBService mongo){
        Console.WriteLine("=== DELIVERERS ===");
        ShowUsersOperation showUsers = new ShowUsersOperation();
        showUsers.Notify += EventListener.OnUserOperation;
        showUsers.Operation(mongo, loggedIn, new MongoDBOperationEventArgs());
    }

    public static void AddDeliverer(MongoDBService mongo){
        Console.WriteLine("=== ADD DELIVERER ===");
        AddUserOperation addUser = new AddUserOperation();
        addUser.Notify += EventListener.OnUserOperation;
        PersonModel addedUser = new PersonModel(GetInputString("Enter username:"), GetInputString("Enter password:"));
        foreach (var databasePerson in mongo.collectionUsers.Find(new BsonDocument()).ToList())
        {
            if (databasePerson.Username == addedUser.Username)
            {
                if(!databasePerson.Roles.Contains(Role.InpostEmployee))
                {
                    databasePerson.AddRole(Role.InpostEmployee);
                    var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, databasePerson.Username);
                    var update = Builders<PersonModel>.Update.Set(r => r.Roles, databasePerson.Roles);
                    mongo.collectionUsers.UpdateOne(filter, update);
                    Console.WriteLine("Added role deliverer to user.");
                    return;
                }
            }
        }
        addedUser.AddRole(Role.InpostEmployee);
        addUser.Operation(mongo, addedUser, new MongoDBOperationEventArgs());
    }

    public static void DeleteDeliverer(MongoDBService mongo){
        Console.WriteLine("=== DELETE DELIVERER ===");
        DeleteUserOperation deleteUser = new DeleteUserOperation();
        deleteUser.Notify += EventListener.OnUserOperation;
        PersonModel deletedUser = new PersonModel(GetInputString("Enter username:"), "");
        foreach (var databasePerson in mongo.collectionUsers.Find(new BsonDocument()).ToList())
        {
            if (databasePerson.Username == deletedUser.Username)
            {
                if(databasePerson.Roles.Contains(Role.InpostEmployee))
                {
                    databasePerson.Roles.Remove(Role.InpostEmployee);
                }
                else
                {
                    Console.WriteLine("User is not a deliverer.");
                    return;
                }

                if(databasePerson.Roles.Count != 0)
                {
                    var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, databasePerson.Username);
                    var update = Builders<PersonModel>.Update.Set(r => r.Roles, databasePerson.Roles);
                    mongo.collectionUsers.UpdateOne(filter, update);
                    Console.WriteLine("Deleted role deliverer from user.");
                    return;
                    
                }
            }   
        }
        deleteUser.Operation(mongo, deletedUser, new MongoDBOperationEventArgs());
        return;
    }

    public static void ChooseMenu(PersonModel loggedIn, MongoDBService mongo){
        if(loggedIn.Roles.Contains(Role.Administrator)){
            ShowMenu(loggedIn, mongo);
        }
        else{
            ShowNormalMenu(loggedIn, mongo);
        }
    }
    
    public static void Main(string[] args)
    {
        // Database
        MongoDBService mongo = ConnectToDatabase();
        DatabaseSearch.mongo = mongo;
        
        // Log in and show menu
        PersonModel loggedIn = LogIn(mongo);
        ChooseMenu(loggedIn, mongo);
    }
}
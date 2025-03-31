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
        while(true){
            Console.WriteLine("=== MENU ===");
            Console.WriteLine("1. Menage clients");
            Console.WriteLine("2. Menage deliverers");
            Console.WriteLine("3. Menage packages");
            Console.WriteLine("4. Log out");
            Console.WriteLine("5. Exit");
            
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
                    Console.Clear();
                    Console.WriteLine($"Logged out successfully.");
                    LogIn();
                    break;
                case 5:
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
            Console.WriteLine("2. Show client");
            Console.WriteLine("3. Delete client");
            Console.WriteLine("4. Back");
            Console.WriteLine("5. Exit");
            
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
                    ShowMenu(loggedIn, mongo);
                    break;
                case 5:
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
            Console.WriteLine("2. Show deliverer");
            Console.WriteLine("3. Delete deliverer");
            Console.WriteLine("4. Back");
            Console.WriteLine("5. Exit");
            
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
                    ShowMenu(loggedIn, mongo);
                    break;
                case 5:
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
            Console.WriteLine("2. Show package");
            Console.WriteLine("3. Delete package");
            Console.WriteLine("4. Back");
            Console.WriteLine("5. Exit");
            
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
                    ShowMenu(loggedIn, mongo);
                    break;
                case 5:
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
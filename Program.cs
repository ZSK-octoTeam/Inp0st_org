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
        while (!int.TryParse(Console.ReadLine(), out input) || input < 1 || input > 5)
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
    
    public static void ShowMenu()
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
                    ShowClientsMenu();
                    break;
                case 2:
                    ShowDeliverersMenu();
                    break;
                case 3:
                    ShowPackagesMenu();
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine($"Logged out successfully.");
                    LogIn(ConnectToDatabase());
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    public static void ShowClientsMenu()
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
                    //AddClient();
                    break;
                case 2:
                    //ShowClient();
                    break;
                case 3:
                    //DeleteClient();
                    break;
                case 4:
                    ShowMenu();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    public static void ShowDeliverersMenu()
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
                    //AddDeliverer();
                    break;
                case 2:
                    //ShowDeliverer();
                    break;
                case 3:
                    //DeleteDeliverer();
                    break;
                case 4:
                    ShowMenu();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    public static void ShowPackagesMenu()
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
                    ShowMenu();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }
        }
    }
    
    public static void Main(string[] args)
    {
        // Database
        MongoDBService mongo = ConnectToDatabase();
        DatabaseSearch.mongo = mongo;
        
        // Operations
        AddUserOperation addUser = new AddUserOperation();
        addUser.Notify += EventListener.OnUserOperation;
        ShowUserOperation showUser = new ShowUserOperation();
        showUser.Notify += EventListener.OnUserOperation;
        UpdateUserOperation updateUser = new UpdateUserOperation();
        updateUser.Notify += EventListener.OnUserOperation;
        DeleteUserOperation deleteUser = new DeleteUserOperation();
        deleteUser.Notify += EventListener.OnUserOperation;
        
        // Log in and show menu
        PersonModel loggedIn = LogIn(mongo);
        ShowMenu();
    }
}
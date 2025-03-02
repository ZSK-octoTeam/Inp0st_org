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
        while (!int.TryParse(Console.ReadLine(), out input) && input < 0 && input > 6)
        {
            Console.WriteLine("Invalid input. Please enter a number:");
        }
        return input;
    }
    
    public static PersonModel LogIn(MongoDBService mongo)
    {
        while (true)
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
                    }
                
                }
            }
            Console.WriteLine("Log in failed. User not found.");
        }
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
        Console.WriteLine("=== MENU ===\n");
        Console.WriteLine("1. Menage clients");
        Console.WriteLine("2. Menage deliverers");
        Console.WriteLine("3. Menage packages");
        Console.WriteLine("4. Log out");
        Console.WriteLine("5. Exit");
        
        int choice = GetInputInt("Enter your choice:");

        switch (choice)
        {
            case 1:
                //MenageClients();
                break;
            case 2:
                //MenageDeliverers();
                break;
            case 3:
                //MenagePackages();
                break;
            case 4:
                //LogOut();
                break;
            case 5:
                Environment.Exit(0);
                break;
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
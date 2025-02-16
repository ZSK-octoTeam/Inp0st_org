using Inpost_org.Services;
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
    
    public static void LogIn()
    {
        string username = GetInputString("Enter your username:");
        string password = GetInputString("Enter your password:");
        
        //check if user exists in database
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
        System.Threading.Thread.Sleep(2000);
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
                //Exit();
                break;
        }
    }
    
    public static void Main(string[] args)
    {
        MongoDBService mongo = ConnectToDatabase();
        PassphraseMenager.mongo = mongo;
        
        MongoDBOperationHandler mongoOperation = null; 
        mongoOperation += new MongoDBOperationHandler(new AddUserOperation().Operation);
        mongoOperation += new MongoDBOperationHandler(new ShowUserOperation().Operation);
        mongoOperation += new MongoDBOperationHandler(new DeleteUserOperation().Operation);
        
        ShowMenu();
    }
}
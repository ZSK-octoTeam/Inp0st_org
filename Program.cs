using Inpost_org.Services;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;
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
            Console.WriteLine("Invalid input. Please enter a number:");
        }
        return input;
    }
    
    public static void LogIn(MongoDBService mango)
    {
            string username = GetInputString("Enter your username:");
            string password = GetInputString("Enter your password:");
        
            PersonModel person = new PersonModel(username, password);
        
            foreach (var databasePerson in mango.Collection.Find(new BsonDocument()).ToList())
            {
                if (databasePerson.Username == person.Username)
                {
                    if(PassphraseMenager.HashPassword(person.Password) == databasePerson.Password)
                    {
                        Console.WriteLine("Log in successful.");
                        ShowMenu();
                    }
                    else
                    {
                        Console.WriteLine("Log in failed. Wrong password.");
                        LogIn(mango);
                    }
                
                }
            }
            Console.WriteLine("Log in failed. User not found.");
            LogIn(mango);
    }
    
    public static MongoDBService ConnectToDatabase()
    {
        string username = GetInputString("Enter database user:");
        string passphrase = GetInputString("Enter database user passphrase:");
        
        MongoDBService mango = new MongoDBService(username, passphrase);
        
        while (!mango.Connect())
        {
            Console.WriteLine("Connection failed. Try again.");
            username = GetInputString("Enter database user:");
            passphrase = GetInputString("Enter database user passphrase:");
            mango.SetUser(username, passphrase);
        }
        
        Console.WriteLine("Connection successful.");
        return mango;
        System.Threading.Thread.Sleep(2000);
    }
    
    public static void ShowMenu()
    {
        while(true){
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
                    LogIn(PassphraseMenager.mango);
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
            Console.WriteLine("=== CLIENTS MENU ===\n");
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
            Console.WriteLine("=== DELIVERERS MENU ===\n");
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
            Console.WriteLine("=== PACKAGES MENU ===\n");
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
        MongoDBService mango = ConnectToDatabase();
        PassphraseMenager.mango = mango;

        PassphraseMenager.PassphraseVerified += (username, verified) =>
        Console.WriteLine( verified ?$"User: {username} found." : $"User {username} not found.");

        MongoDBOperationHandler mongoOperation = null; 
        mongoOperation += new MongoDBOperationHandler(new AddUserOperation().Operation);
        mongoOperation += new MongoDBOperationHandler(new ShowUserOperation().Operation);
        mongoOperation += new MongoDBOperationHandler(new DeleteUserOperation().Operation);

        LogIn(mango);
        //ParcelModel parcel = new ParcelModel("paczka", new PersonModel("username", "password"), new PersonModel("username", "password"));
        //mango.CollectionParcels.InsertOne(parcel);
    }
}
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
                    LogIn(mongo);
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
        MongoDBService mongo = ConnectToDatabase();
        DatabaseSearch.mongo = mongo;
        
        // User operations
        /*
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
        */
        
        //Parcel operation
        /*
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
        */

        // Log in and show menu
        //PersonModel loggedIn = LogIn(mongo);
        //ShowMenu(loggedIn, mongo);
    }
}
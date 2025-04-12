using Inpost_org.Enums;
using Inpost_org.Services;
using Inpost_org.Users;

namespace Inpost_org.UI;

public class Menus
{
    public static void ShowMenu(PersonModel loggedIn, MongoDBService mongo)
    {
        while(loggedIn.Roles.Contains(Role.Administrator))
        {
            
            Inputs.ShowHeader("=== MENU ===");
            Console.WriteLine("1. Menage clients");
            Console.WriteLine("2. Menage deliverers");
            Console.WriteLine("3. Menage packages");
            Console.WriteLine("4. Menage users");
            Console.WriteLine("5. Log out");
            Console.WriteLine("6. Exit");
            
            int choice = Inputs.GetInputInt("Enter your choice:");

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
                    loggedIn = Inputs.LogIn();
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
            Inputs.ShowHeader("=== CLIENTS MENU ===");
            Console.WriteLine("1. Add client");
            Console.WriteLine("2. Show all clients");
            Console.WriteLine("3. Delete client");
            Console.WriteLine("4. Update client");
            Console.WriteLine("5. Search client");
            Console.WriteLine("6. Back");
            Console.WriteLine("7. Exit");
            
            int choice = Inputs.GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    OperationEngine.AddClient(mongo);
                    break;
                case 2:
                    OperationEngine.ShowClients(loggedIn, mongo);
                    break;
                case 3:
                    OperationEngine.DeleteClient(mongo);
                    break;
                case 4:
                    OperationEngine.UpdateClient(mongo);
                    break;
                case 5:
                    OperationEngine.SearchClient(mongo);
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
            Inputs.ShowHeader("=== DELIVERERS MENU ===");
            Console.WriteLine("1. Add deliverer");
            Console.WriteLine("2. Show all deliverers");
            Console.WriteLine("3. Delete deliverer");
            Console.WriteLine("4. Update deliverer");
            Console.WriteLine("5. Search deliverer");
            Console.WriteLine("6. Back");
            Console.WriteLine("7. Exit");
            
            int choice = Inputs.GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    OperationEngine.AddDeliverer(mongo);
                    break;
                case 2:
                    OperationEngine.ShowDeliverers(loggedIn, mongo);
                    break;
                case 3:
                    OperationEngine.DeleteDeliverer(mongo);
                    break;
                case 4:
                    OperationEngine.UpdateDeliverer(mongo);
                    break;
                case 5:
                    OperationEngine.SearchDeliverer(mongo);
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
            Inputs.ShowHeader("=== PACKAGES MENU ===");
            Console.WriteLine("1. Add package");
            Console.WriteLine("2. Show all packages");
            Console.WriteLine("3. Delete package");
            Console.WriteLine("4. Update package");
            Console.WriteLine("5. Search package");
            Console.WriteLine("6. Back");
            Console.WriteLine("7. Exit");
            
            int choice = Inputs.GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    OperationEngine.AddPackage(mongo);
                    break;
                case 2:
                    OperationEngine.ShowPackages(loggedIn, mongo);
                    break;
                case 3:
                    OperationEngine.DeletePackage(loggedIn, mongo);
                    break;
                case 4:
                    OperationEngine.UpdatePackage(loggedIn ,mongo);
                    break;
                case 5:
                    OperationEngine.SearchPackage(loggedIn, mongo);
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
            Inputs.ShowHeader("=== MENU ===");
            Console.WriteLine("1. Menage packages");
            Console.WriteLine("2. Log out");
            Console.WriteLine("3. Exit");
            
            int choice = Inputs.GetInputInt("Enter your choice:");

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
                    loggedIn = Inputs.LogIn();
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
            Inputs.ShowHeader("=== Customer and Deliverer Menu ===");
            Console.WriteLine("1. Menage packages(deliverer)");
            Console.WriteLine("2. Menage packages(client)");
            Console.WriteLine("3. Log out");
            Console.WriteLine("4. Exit");

            int choice = Inputs.GetInputInt("Enter your choice:");
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
                    loggedIn = Inputs.LogIn();
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
            Inputs.ShowHeader("=== USERS MENU ===");
            Console.WriteLine("1. Add user");
            Console.WriteLine("2. Show all users");
            Console.WriteLine("3. Update user");
            Console.WriteLine("4. Delete user");
            Console.WriteLine("5. Search user");
            Console.WriteLine("6. Back");
            Console.WriteLine("7. Exit");
            
            int choice = Inputs.GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    OperationEngine.AddUser(mongo);
                    break;
                case 2:
                    OperationEngine.ShowUsers(loggedIn, mongo);
                    break;
                case 3:
                    OperationEngine.UpdateUser(mongo);
                    break;
                case 4:
                    OperationEngine.DeleteUser(mongo);
                    break;
                case 5:
                    OperationEngine.SearchUser(mongo);
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
            Inputs.ShowHeader("=== PACKAGES MENU ===");
            Console.WriteLine("1. Order package");
            Console.WriteLine("2. Show all my packages");
            Console.WriteLine("3. Cancel package");
            Console.WriteLine("4. Search package");
            Console.WriteLine("5. Back");
            Console.WriteLine("6. Exit");
            
            int choice = Inputs.GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    OperationEngine.OrderPackage(loggedIn, mongo);
                    break;
                case 2:
                    OperationEngine.ShowMyPackages(loggedIn, mongo);
                    break;
                case 3:
                    OperationEngine.DeletePackage(loggedIn, mongo);
                    break;
                case 4:
                    OperationEngine.SearchPackage(loggedIn, mongo);
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
            Inputs.ShowHeader("=== PACKAGES MENU ===");
            Console.WriteLine("1. Show all my packages");
            Console.WriteLine("2. Show all packages");
            Console.WriteLine("3. Pick up package");
            Console.WriteLine("4. Deliver package");
            Console.WriteLine("5. Search package");
            Console.WriteLine("6. Back");
            Console.WriteLine("7. Exit");
            
            int choice = Inputs.GetInputInt("Enter your choice:");

            switch (choice)
            {
                case 1:
                    OperationEngine.ShowMyPackages(loggedIn, mongo);
                    break;
                case 2:
                    OperationEngine.ShowPackages(loggedIn, mongo);
                    break;
                case 3:
                    OperationEngine.PickUpPackage(loggedIn, mongo);
                    break;
                case 4:
                    OperationEngine.DeliverPackage(loggedIn, mongo);
                    break;
                case 5:
                    OperationEngine.SearchPackage(loggedIn, mongo);
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
}
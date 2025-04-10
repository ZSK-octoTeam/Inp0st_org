using System.Text;
using Inpost_org.Services;
using Inpost_org.Users;

namespace Inpost_org.UI;

public class Inputs
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
}
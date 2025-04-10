using Inpost_org.Services;
using Inpost_org.UI;
using Inpost_org.Users;

internal class Program
{
    public static void Main(string[] args)
    {
        // Database connection
        MongoDBService mongo = new MongoDBService();
        mongo.Connect();
        DatabaseSearch.mongo = mongo;
        
        // Log in site and menu
        PersonModel loggedIn = Inputs.LogIn();
        Menus.ChooseMenu(loggedIn, mongo);
    }
}
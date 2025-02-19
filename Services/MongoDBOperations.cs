using Inpost_org.Users;

namespace Inpost_org.Services;

public delegate void MongoDBOperationHandler(MongoDBService mongo, PersonModel person);

public class AddUserOperation : CRUD
{
    public void Operation(MongoDBService mongo, PersonModel person)
    {
        mongo.Collection.InsertOne(person);
        Console.WriteLine("User added.");
    }
}

public class ShowUserOperation : CRUD
{
    public void Operation(MongoDBService mongo, PersonModel person)
    {
        //add show details about person
        //mongo.ShowUserDetails();
        Console.WriteLine("User details shown.");
    }
}

public class DeleteUserOperation : CRUD
{
    public void Operation(MongoDBService mongo, PersonModel person)
    {
        //add delete person
        Console.WriteLine("User deleted.");
    }
}
using Inpost_org.Users;
using MongoDB.Driver;

namespace Inpost_org.Services;

public delegate void MongoDBOperationHandler(MongoDBService mongo, PersonModel person);

public class AddUserOperation : CRUD
{
    public void Operation(MongoDBService mongo, PersonModel person)
    {
        if (!PassphraseMenager.VerifyUser(person))
        {
            person.Password = PassphraseMenager.HashPassword(person.Password);
            mongo.Collection.InsertOne(person);
            Console.WriteLine("User added.");
        }
        else
        {
            Console.WriteLine("User already exists.");
        }
    }
}

public class ShowUserOperation : CRUD
{
    public void Operation(MongoDBService mongo, PersonModel person)
    {
        if (PassphraseMenager.VerifyUser(person))
        {
            var rbac = new RBAC();
            Console.WriteLine($"Username: {person.Username}");
            Console.WriteLine("Role: ");
            foreach (var role in person.Roles)
            {
                Console.WriteLine(role);
            }
            Console.WriteLine("Has permission to: ");
            foreach (var permission in Enum.GetValues(typeof(Permission)))
            {
                if (rbac.HasPermission(person, (Permission)permission))
                {
                    Console.WriteLine(permission);
                }
            }
        }
        else
        {
            Console.WriteLine("There is nothing to show.");
        }
    }
}

public class DeleteUserOperation : CRUD
{
    public void Operation(MongoDBService mongo, PersonModel person)
    {
        if (PassphraseMenager.VerifyUser(person))
        {
            var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);
            mongo.Collection.DeleteOne(filter);
            Console.WriteLine("User deleted.");
        }
        else
        {
            Console.WriteLine("User could not be deleted.");
        }
    }
}

public class UpdateUserOperation : CRUD
{
    public void Operation(MongoDBService mongo, PersonModel person)
    {
        if (PassphraseMenager.VerifyUser(person))
        {
            var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);
            var update = Builders<PersonModel>.Update.Set(r => r.Password, PassphraseMenager.HashPassword(person.Password));
            mongo.Collection.UpdateOne(filter, update);
            Console.WriteLine("User updated.");
        }
        else
        {
            Console.WriteLine("User could not be updated.");
        }
    }
}
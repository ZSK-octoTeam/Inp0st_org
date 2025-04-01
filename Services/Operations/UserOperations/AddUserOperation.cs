using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

public class AddUserOperation : UserBase
{
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e, string role)
    {
        e.Operation = "AddUser";
        var users = DatabaseSearch.FindUsers();
        if (!users.ContainsKey(person.Username))
        {
            person.Password = DatabaseSearch.HashPassword(person.Password);
            if(person.Roles.Count == 0)
            {
                person.Roles.Add(Enum.Parse<Role>(role));
            }
            mongo.collectionUsers.InsertOne(person);
            e.Success = true;
        }
        else
        {
            string wynik = string.Join(", ", person.Roles.Select(e => e.ToString()));
            if(wynik.Contains(role) || users[person.Username].Roles.Contains(Enum.Parse<Role>(role)))
            {
                e.Success = false;
                e.Message = "User already exists.";
            }
            else
            {
                //naprawic bo mozna dodac 2 takie same role
                users[person.Username].Roles.Add(Enum.Parse<Role>(role));
                var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);
                var update = Builders<PersonModel>.Update.Set(r => r.Roles, users[person.Username].Roles);
                mongo.collectionUsers.UpdateOne(filter, update);
                e.Success = true;
            }
        }

        OnNotify(person, e);
    }
}
using Inpost_org.Enums;
using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

/// <summary>
/// Class responsible for adding users to the MongoDB database.
/// </summary>
public class AddUserOperation : UserBase
{
    /// <summary>
    /// Executes the operation of adding a user to the database.
    /// </summary>
    /// <param name="mongo">The MongoDB service object.</param>
    /// <param name="person">The person model representing the user to be added.</param>
    /// <param name="e">The event arguments for the MongoDB operation.</param>
    /// <param name="role">The role to be assigned to the user.</param>
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
                users[person.Username].Roles.Add(Enum.Parse<Role>(role));
                var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);
                var update = Builders<PersonModel>.Update.Set(r => r.Roles, users[person.Username].Roles);
                mongo.collectionUsers.UpdateOne(filter, update);
                e.Success = true;
                e.Message = $"Role: {role} added to user {person.Username}";
            }
        }

        OnNotify(person, e);
    }
}
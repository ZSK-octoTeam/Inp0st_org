using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;
using Inpost_org.Users.Deliveries;
using MongoDB.Driver;

namespace Inpost_org.Services.Operations.UserOperations;

public class DeleteUserOperation : UserBase
{
    public override void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e, string role)
    {
        e.Operation = "DeleteUser";
        var users = DatabaseSearch.FindUsers();
        if (users.ContainsKey(person.Username))
        {
            string wynik = string.Join(", ", users[person.Username].Roles.Select(e => e.ToString()));
            if (!wynik.Contains(role))
            {
                e.Success = false;
                e.Message = $"User is not a {role}.";
            }
            else
            {
                if (users[person.Username].Roles.Count == 1 || !Enum.TryParse<Role>(role, out var result))
                {
                    var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);
                    mongo.collectionUsers.DeleteOne(filter);
                    e.Success = true;

                    foreach (var databaseParcel in DatabaseSearch.FindParcels())
                    {
                        if (databaseParcel.Value.Recipient  != null && databaseParcel.Value.Recipient.Username == person.Username)
                        {
                            var fill = Builders<ParcelModel>.Filter.Eq(r => r.Recipient.Username, person.Username);
                            var upt = Builders<ParcelModel>.Update.Set(r => r.Recipient, null);
                            mongo.collectionParcels.UpdateOne(fill, upt);
                        }

                        if (databaseParcel.Value.Sender != null && databaseParcel.Value.Sender.Username == person.Username)
                        {
                            var fill = Builders<ParcelModel>.Filter.Eq(r => r.Sender.Username, person.Username);
                            var upt = Builders<ParcelModel>.Update.Set(r => r.Sender, null);
                            mongo.collectionParcels.UpdateOne(fill, upt);
                        }
                    }
                }
                else
                {
                    users[person.Username].Roles.Remove(Enum.Parse<Role>(role));
                    var filter = Builders<PersonModel>.Filter.Eq(r => r.Username, person.Username);
                    var update = Builders<PersonModel>.Update.Set(r => r.Roles, users[person.Username].Roles);
                    mongo.collectionUsers.UpdateOne(filter, update);
                    e.Success = true;

                    foreach (var databaseParcel in DatabaseSearch.FindParcels())
                    {
                        if (Enum.Parse<Role>(role) == Role.InpostClient)
                        {
                            if (databaseParcel.Value.Recipient != null && databaseParcel.Value.Recipient.Username == person.Username)
                            {
                                var fill = Builders<ParcelModel>.Filter.Eq(r => r.Recipient.Username, person.Username);
                                var upt = Builders<ParcelModel>.Update.Set(r => r.Recipient, null);
                                mongo.collectionParcels.UpdateOne(fill, upt);
                            }
                        }

                        if (Enum.Parse<Role>(role) == Role.InpostEmployee)
                        {
                            if (databaseParcel.Value.Sender != null && databaseParcel.Value.Sender.Username == person.Username)
                            {
                                var fill = Builders<ParcelModel>.Filter.Eq(r => r.Sender.Username, person.Username);
                                var upt = Builders<ParcelModel>.Update.Set(r => r.Sender, null);
                                mongo.collectionParcels.UpdateOne(fill, upt);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            e.Success = false;
            e.Message = "User does not exist.";
        }

        OnNotify(person, e);
    }
}
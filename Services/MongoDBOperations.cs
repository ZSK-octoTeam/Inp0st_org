using Inpost_org.Users;

namespace Inpost_org.Services;

public delegate void MongoDBOperationHandler(MongoDBService mongo, PersonModel person);

public class MongoDBOperation : CRUD
{
    public void Operation(MongoDBService mongo, PersonModel person)
    {
        mongo.Collection.InsertOne(person);
    }
}

public class ShowUsersOperation : CRUD
{
    public void Operation(MongoDBService mongo, PersonModel person)
    {
        //add show details about person
        //mongo.ShowUserDetails();
    }
}

public class DeletePersonOperation : CRUD
{
    public void Operation(MongoDBService mongo, PersonModel person)
    {
        //add delete person
    }
}
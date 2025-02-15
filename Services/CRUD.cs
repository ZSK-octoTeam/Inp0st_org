using Inpost_org.Users;

namespace Inpost_org.Services;

public interface CRUD
{
    void Operation(MongoDBService mongo, PersonModel person);
}
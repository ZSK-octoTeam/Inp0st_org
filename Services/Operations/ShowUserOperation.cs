using Inpost_org.Services.NotificationMethods;
using Inpost_org.Users;

namespace Inpost_org.Services.Operations;

public class ShowUserOperation
{
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public event MongoDBOperationHandler Notify;

    public void Operation(MongoDBService mongo, PersonModel person, MongoDBOperationEventArgs e)
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

            Success = true;
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
            Success = false;
            Message = "There is nothing to show.";
        }

        Notify?.Invoke(this, person, new MongoDBOperationEventArgs("show", Success, Message));
    }
}
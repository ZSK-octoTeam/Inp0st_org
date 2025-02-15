using Inpost_org.Users;

namespace Inpost_org.Services;

public class CRUDMenager
{
    public MongoDBOperationHandler Operations;

            public void AddOperationMethod(MongoDBOperationHandler handler)
            {
                Console.WriteLine("Dodano operację!");
                Operations += handler;
            }

            public void RemoveOperationMethod(MongoDBOperationHandler handler)
            {
                if (Operations != null && Operations.GetInvocationList().Contains(handler))
                {
                    Operations -= handler;
                    Console.WriteLine("Usunięto operację!");
                    return;
                }
                Console.WriteLine("Brak oporacji do usunięcia...");
            }

            public void ApplyOperations(MongoDBService mongo, PersonModel person)
            {
                if (Operations == null)
                {
                    Console.WriteLine("Brak zarejestrowanych operacji na bazie danych...");
                    return;
                }

                foreach (var handler in Operations.GetInvocationList())
                {
                    try
                    {
                        handler.DynamicInvoke(mongo, person);
                    }
                    catch
                    {
                        Console.WriteLine("Błąd podczas wykonywania operacji...");
                    }
                }
            }

            public void ListOperationMethod()
            {
                if (Operations == null)
                {
                    Console.WriteLine("Brak zarejestrowanych operacji na bazie danych...");
                    return;
                }

                Console.WriteLine("Zarajestrowane operacje: ");

                var displayhadler = new HashSet<string>();

                foreach (var handler in Operations.GetInvocationList())
                {
                    var target = handler.Target;
                    var methodName = handler.Method.Name;
                    var className = target.GetType().FullName;

                    var uniqeKey = $"{className}, {methodName}";

                    displayhadler.Add(uniqeKey);
                }

                foreach (var handler in displayhadler)
                {
                    Console.WriteLine(handler);
                }
            }
}
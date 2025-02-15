namespace Inpost_org.Users;

public enum Permission
{
    Read,
    Write,
    Edit,
    ManageUsers
}

public class RBAC
{
    private readonly Dictionary<Role, List<Permission>> _rolePermissions;
    
    public RBAC()
    {
        _rolePermissions = new Dictionary<Role, List<Permission>>
        {
            { Role.Administrator, new List<Permission>{ Permission.Read, Permission.Write, Permission.Edit, Permission.ManageUsers }},
            { Role.InpostEmployee, new List<Permission>{ Permission.Read, Permission.Write}},
            { Role.InpostClient, new List<Permission>{ Permission.Read, Permission.Edit } }
        };
    }
    
    public bool HasPermission(PersonModel user, Permission permission)
    {
        foreach (var role in user.Roles)
        {
            if (_rolePermissions.ContainsKey(role) && _rolePermissions[role].Contains(permission))
            {
                return true;
            }
        }

        return false;
    }
}
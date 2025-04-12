using Inpost_org.Enums;

namespace Inpost_org.Users;

public class RBAC
{
    private readonly Dictionary<Role, List<Permission>> _rolePermissions;
    
    /// <summary>
    /// permission dictionary for users
    /// </summary>
    public RBAC()
    {
        _rolePermissions = new Dictionary<Role, List<Permission>>
        {
            { Role.Administrator, new List<Permission>{ Permission.DeletePackages, Permission.OrderPackages, Permission.DeliverPackages, Permission.ManageUsers, Permission.UpdatePackages }},
            { Role.InpostEmployee, new List<Permission>{ Permission.DeletePackages, Permission.UpdatePackages}},
            { Role.InpostClient, new List<Permission>{ Permission.DeletePackages, Permission.OrderPackages } }
        };
    }
    
    /// <summary>
    /// checks user permission
    /// </summary>
    /// <param name="user"></param>
    /// <param name="permission"></param>
    /// <returns>return true if user has permission</returns>
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
namespace CIS.Security.InternalServices;

internal record CisUser(int Id, string Name, string Login) 
    : Core.Security.ICurrentUser
{
}

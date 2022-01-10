namespace CIS.Infrastructure.Security;

public record CisUser(int Id, string Name, string Login) 
    : CIS.Core.Security.ICurrentUser
{
}

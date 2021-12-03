namespace CIS.Infrastructure.Security
{
    public record CisUser(int Id, string Name) : CIS.Core.Security.ICurrentUser
    {
    }
}

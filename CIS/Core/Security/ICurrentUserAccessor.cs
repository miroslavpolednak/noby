namespace CIS.Core.Security;

public interface ICurrentUserAccessor
{
    ICurrentUser? User { get; }
}
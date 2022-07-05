namespace CIS.Core.Security;

public interface ICurrentUserDetails
{
    string DisplayName { get; }
    string? CPM { get; }
    string? ICP { get; }
}

namespace CIS.Core.Security;

public class CisUserDetails
    : ICurrentUserDetails
{
    public string DisplayName { get; set; } = string.Empty;

    public string? CPM { get; set; }

    public string? ICP { get; set; }
}

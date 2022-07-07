namespace CIS.Core.Security;

public class CisUserDetails
    : ICurrentUserDetails
{
    public string DisplayName { get; set; } = string.Empty;
}

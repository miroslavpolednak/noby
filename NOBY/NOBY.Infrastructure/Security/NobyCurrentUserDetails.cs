namespace NOBY.Infrastructure.Security;

public sealed class NobyCurrentUserDetails
: CIS.Core.Security.CisUserDetails, CIS.Foms.Types.Interfaces.IFomsCurrentUserDetails
{
    public string? CPM { get; set; }

    public string? ICP { get; set; }
}

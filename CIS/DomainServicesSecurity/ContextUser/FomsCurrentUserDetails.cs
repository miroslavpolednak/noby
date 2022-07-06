namespace CIS.DomainServicesSecurity.ContextUser;

public sealed class FomsCurrentUserDetails
    : Core.Security.CisUserDetails, Foms.Types.Interfaces.IFomsCurrentUserDetails
{
    public string? CPM { get; set; }

    public string? ICP { get; set; }
}

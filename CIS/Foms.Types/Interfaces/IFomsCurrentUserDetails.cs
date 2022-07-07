using CIS.Core.Security;

namespace CIS.Foms.Types.Interfaces;

public interface IFomsCurrentUserDetails
    : ICurrentUserDetails
{
    string? CPM { get; }

    string? ICP { get; }
}

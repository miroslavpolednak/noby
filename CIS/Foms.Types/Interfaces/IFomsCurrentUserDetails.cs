using CIS.Core.Security;

namespace CIS.Foms.Types.Interfaces;

/// <summary>
/// Detail uživatele aplikace NOBY
/// </summary>
public interface IFomsCurrentUserDetails
    : ICurrentUserDetails
{
    string? CPM { get; }

    string? ICP { get; }
}

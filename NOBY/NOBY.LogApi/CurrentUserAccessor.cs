using CIS.Core.Security;
using System.Security.Claims;

namespace NOBY.LogApi;

internal sealed class CurrentUserAccessor
    : ICurrentUserAccessor
{
    public IEnumerable<Claim> Claims => new List<Claim>();

    public bool IsAuthenticated => true;

    public ICurrentUser? User => new CurrentUser();

    public ICurrentUserDetails? UserDetails => throw new NotImplementedException();

    public Task<ICurrentUserDetails> EnsureDetails(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TDetails> EnsureDetails<TDetails>(CancellationToken cancellationToken = default) where TDetails : ICurrentUserDetails
    {
        throw new NotImplementedException();
    }
}

internal sealed class CurrentUser
    : ICurrentUser
{
    public int Id => 1;

    public string? Login => "FeApiLogger";
}
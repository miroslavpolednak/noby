using CIS.Core.Security;
using System.Security.Claims;

namespace Console_CustomerService;

public sealed class MockCurrentUserAccessor 
    : ICurrentUserAccessor
{
    public Task<ICurrentUserDetails> EnsureDetails(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TDetails> EnsureDetails<TDetails>(CancellationToken cancellationToken) where TDetails : ICurrentUserDetails
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Claim> Claims
    {
        get => new List<Claim>();
    }

    public bool IsAuthenticated => true;
    public ICurrentUser? User => new CurrentUser();
    public ICurrentUserDetails? UserDetails => new CisUserDetails { DisplayName = "Test" };
}
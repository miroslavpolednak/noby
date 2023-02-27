using System.Security.Claims;
using CIS.Core.Security;

namespace Console_CustomerService;

public class MockCurrentUserAccessor : ICurrentUserAccessor
{
    public Task<ICurrentUserDetails> EnsureDetails(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TDetails> EnsureDetails<TDetails>(CancellationToken cancellationToken) where TDetails : ICurrentUserDetails
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Claim> Claims { get; } = Enumerable.Empty<Claim>();

    public bool IsAuthenticated => true;
    public ICurrentUser? User => new CurrentUser();
    public ICurrentUserDetails? UserDetails => new CisUserDetails { DisplayName = "Test" };
}
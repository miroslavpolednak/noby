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

    public bool IsAuthenticated => true;
    public ICurrentUser? User => new CurrentUser();
    public ICurrentUserDetails? UserDetails => new CisUserDetails { DisplayName = "Test" };
}
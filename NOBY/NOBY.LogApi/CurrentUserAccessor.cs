using CIS.Core.Security;
using System.Security.Claims;

namespace NOBY.LogApi;

internal sealed class CurrentUserAccessor
    : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContext;

    public CurrentUserAccessor(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public IEnumerable<Claim> Claims => _httpContext.HttpContext!.User.Claims;

    public bool IsAuthenticated => true;

    public ICurrentUser? User => new CurrentUser(
        Convert.ToInt32(_httpContext.HttpContext!.User.Claims.First(t => t.Type == SecurityConstants.ClaimTypeId).Value, System.Globalization.CultureInfo.InvariantCulture),
        _httpContext.HttpContext.User.Claims.First(t => t.Type == SecurityConstants.ClaimTypeIdent).Value
    );

    public ICurrentUserDetails? UserDetails => throw new NotImplementedException();
    public Task<ICurrentUserDetails> EnsureDetails(CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public Task<TDetails> EnsureDetails<TDetails>(CancellationToken cancellationToken = default) where TDetails : ICurrentUserDetails => throw new NotImplementedException();
}

internal sealed class CurrentUser
    : ICurrentUser
{
    public CurrentUser(int id, string login)
    {
        Id = id;
        Login = login;
    }

    public int Id { get; private set; }
    public string? Login { get; private set; }
}
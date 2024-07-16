using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.v1.GetCasesByIdentity;

internal class GetCasesByIdentityHandler : IRequestHandler<GetCasesByIdentityRequest, GetCasesByIdentityResponse>
{
    private readonly CaseServiceDbContext _dbContext;

    public GetCasesByIdentityHandler(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetCasesByIdentityResponse> Handle(GetCasesByIdentityRequest request, CancellationToken cancellationToken)
    {
        var identity = request.CustomerIdentity;
        var cases = await _dbContext.Cases
                                    .AsNoTracking()
                                    .Where(t => t.CustomerIdentityId == identity.IdentityId && (byte?)t.CustomerIdentityScheme == (byte)identity.IdentityScheme)
                                    .Select(DatabaseExpressions.CaseDetail())
                                    .ToListAsync(cancellationToken);

        return new GetCasesByIdentityResponse { Cases = { cases } };
    }
}
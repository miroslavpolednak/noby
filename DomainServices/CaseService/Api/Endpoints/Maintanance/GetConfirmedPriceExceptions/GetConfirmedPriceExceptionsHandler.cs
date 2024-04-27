using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.Maintanance.GetConfirmedPriceExceptions;

internal sealed class GetConfirmedPriceExceptionsHandler(CaseServiceDbContext _dbContext)
    : IRequestHandler<GetConfirmedPriceExceptionsRequest, GetConfirmedPriceExceptionsResponse>
{
    public async Task<GetConfirmedPriceExceptionsResponse> Handle(GetConfirmedPriceExceptionsRequest request, CancellationToken cancellationToken)
    {
        var list = await _dbContext
            .ConfirmedPriceExceptions
            .AsNoTracking()
            .Where(t => t.ConfirmedDate < request.OlderThan)
            .Select(t => t.CaseId)
            .ToListAsync(cancellationToken);
        
        var response = new GetConfirmedPriceExceptionsResponse();
        response.CaseId.AddRange(list);
        return response;
    }
}

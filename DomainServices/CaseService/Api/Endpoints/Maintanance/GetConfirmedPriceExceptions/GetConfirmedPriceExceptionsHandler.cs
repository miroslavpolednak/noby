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
            .Where(t => t.ConfirmedDate != null || t.DeclinedDate < request.OlderThan)
            .Select(t => new { t.CaseId, t.TaskIdSB })
            .ToListAsync(cancellationToken);

        var response = new GetConfirmedPriceExceptionsResponse();
        response.ConfirmedPriceExp.AddRange(list.Select(s => new ConfirmedPriceException { CaseId = s.CaseId, TaskIdSB = s.TaskIdSB }));
        return response;
    }
}

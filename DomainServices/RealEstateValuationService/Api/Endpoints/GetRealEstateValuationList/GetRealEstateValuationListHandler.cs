using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.GetRealEstateValuationList;

internal sealed class GetRealEstateValuationListHandler
    : IRequestHandler<GetRealEstateValuationListRequest, GetRealEstateValuationListResponse>
{
    public async Task<GetRealEstateValuationListResponse> Handle(GetRealEstateValuationListRequest request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.RealEstateValuations
            .AsNoTracking()
            .Where(t => t.CaseId == request.CaseId)
            .Select(Database.Mappers.RealEstateDetail())
            .ToListAsync(cancellationToken);

        var response = new GetRealEstateValuationListResponse();
        response.RealEstateValuationList.AddRange(list);
        return response;
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public GetRealEstateValuationListHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}

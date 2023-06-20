using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.GetRealEstateValuationList;

internal sealed class GetRealEstateValuationListHandler
    : IRequestHandler<GetRealEstateValuationListRequest, GetRealEstateValuationListResponse>
{
    public async Task<GetRealEstateValuationListResponse> Handle(GetRealEstateValuationListRequest request, CancellationToken cancellationToken)
    {
        return new GetRealEstateValuationListResponse();
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public GetRealEstateValuationListHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}

using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.CreateRealEstateValuation;

internal sealed class CreateRealEstateValuationHandler
    : IRequestHandler<CreateRealEstateValuationRequest, CreateRealEstateValuationResponse>
{
    public async Task<CreateRealEstateValuationResponse> Handle(CreateRealEstateValuationRequest request, CancellationToken cancellationToken)
    {

        return new CreateRealEstateValuationResponse
        {
            NobyOrderId = 1
        };
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public CreateRealEstateValuationHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}

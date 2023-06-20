using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Clients;

public interface IRealEstateValuationServiceClient
{
    Task<int> CreateRealEstateValuation(CreateRealEstateValuationRequest request, CancellationToken cancellationToken = default);

    Task DeleteRealEstateValuation(long caseId, int nobyOrderId, CancellationToken cancellationToken = default);

    Task<GetRealEstateValuationListResponse> GetRealEstateValuationList(long caseId, CancellationToken cancellationToken = default);
}

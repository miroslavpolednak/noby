using DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.Dto;
using DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.V1.Contracts;

namespace DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.V1;

internal sealed class MockLuxpiServiceClient
    : ILuxpiServiceClient
{
    public Task<CreateKbmodelFlatResponse> CreateKbmodelFlat(KBModelRequest request, long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

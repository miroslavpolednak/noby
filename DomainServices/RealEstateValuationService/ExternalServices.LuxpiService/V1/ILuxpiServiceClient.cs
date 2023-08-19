using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.V1;

public interface ILuxpiServiceClient
    : IExternalServiceClient
{
    const string Version = "V1";

    Task<Dto.CreateKbmodelFlatResponse> CreateKbmodelFlat(Contracts.KBModelRequest request, long id, CancellationToken cancellationToken = default);
}

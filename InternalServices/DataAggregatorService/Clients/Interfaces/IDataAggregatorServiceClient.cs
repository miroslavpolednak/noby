using CIS.InternalServices.DataAggregatorService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Clients;

public interface IDataAggregatorServiceClient
{
    Task<GetDocumentDataResponse> GetDocumentData(GetDocumentDataRequest request, CancellationToken cancellationToken = default);

    Task<GetEasFormResponse> GetEasForm(GetEasFormRequest request, CancellationToken cancellationToken = default);
}
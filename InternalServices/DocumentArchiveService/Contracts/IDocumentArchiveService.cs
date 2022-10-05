using System.ServiceModel;

namespace CIS.InternalServices.DocumentArchiveService.Contracts;

[ServiceContract(Name = "CIS.InternalServices.DocumentArchiveService.Contracts.V1")]
public interface IDocumentArchiveService
{
    ValueTask<GenerateDocumentIdResponse> GenerateDocumentId(GenerateDocumentIdRequest request, CancellationToken cancellationToken = default);
}

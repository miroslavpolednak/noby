using System.ServiceModel;

namespace DomainServices.DocumentArchiveService.Contracts;

[ServiceContract(Name = "DomainServices.DocumentArchiveService.Contracts.V1")]
public interface IDocumentArchiveService
{
    ValueTask<GenerateDocumentIdResponse> GenerateDocumentId(GenerateDocumentIdRequest request, CancellationToken cancellationToken = default);
}

using CIS.Core.Results;
using CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace CIS.InternalServices.DocumentGeneratorService.Clients;

public interface      IDocumentGeneratorServiceClient
{
    Task<IServiceCallResult> GenerateDocument(GenerateDocumentRequest request, CancellationToken cancellationToken = default);
}
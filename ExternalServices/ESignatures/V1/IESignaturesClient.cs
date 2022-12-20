using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.ESignatures.V1;

public interface IESignaturesClient
    : IExternalServiceClient
{
    Task<string?> GetDocumentStatus(string documentId, IdentitySchemes mandant, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";
}
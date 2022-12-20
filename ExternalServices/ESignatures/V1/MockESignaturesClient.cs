using CIS.Foms.Enums;

namespace ExternalServices.ESignatures.V1;

internal sealed class MockESignaturesClient 
    : IESignaturesClient
{
    public Task<string?> GetDocumentStatus(string documentId, IdentitySchemes mandant, CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }
}

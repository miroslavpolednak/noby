using ExternalServices.ESignatures.V1.ESignaturesWrapper;

namespace ExternalServices.ESignatures.V1
{
    internal sealed class MockESignaturesClient : IESignaturesClient
    {
        public Task<IServiceCallResult> PrepareDocument(PrepareDocumentRequest request, string org)
        {
            throw new NotImplementedException();
        }
    }
}

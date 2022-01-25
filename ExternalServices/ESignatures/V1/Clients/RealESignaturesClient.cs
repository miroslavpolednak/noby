using ExternalServices.ESignatures.V1.ESignaturesWrapper;

namespace ExternalServices.ESignatures.V1
{
    internal sealed class RealESignaturesClient : BaseClient<RealESignaturesClient>, IESignaturesClient
    {
        public RealESignaturesClient(HttpClient httpClient, ILogger<RealESignaturesClient> logger) : base(httpClient, logger) { }

        public Task<IServiceCallResult> PrepareDocument(PrepareDocumentRequest request, string org)
        {
            throw new NotImplementedException();
        }
    }
}

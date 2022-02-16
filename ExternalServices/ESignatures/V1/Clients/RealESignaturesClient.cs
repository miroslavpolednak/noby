using CIS.Foms.Enums;
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

        public async Task<IServiceCallResult> GetDocumentStatus(string documentId, IdentitySchemes mandant)
        {
            _logger.LogDebug("Run inputs: ESignatures GetDocumentStatus [DocumentId: {id}, Mandant: {mandant}]", documentId, mandant);

            return await WithDocumentClient(async c =>
            {
                return await callMethod(async () =>
                {
                    var result = await c.GetDocumentStatusAsync(documentId, ToOrg(mandant));
                    return new SuccessfulServiceCallResult<ResponseStatus>(result);
                });
            });
        }

        private string ToOrg(IdentitySchemes mandant)
        {
            if (mandant != IdentitySchemes.Mp)
            {
                throw new NotSupportedException($"Mandant '{mandant}' is not supported");
            }

            return "mpss";
        }

        private DocumentServiceClient CreateClient()
           => new(_httpClient);

        private async Task<IServiceCallResult> WithDocumentClient(Func<DocumentServiceClient, Task<IServiceCallResult>> fce)
        {
            try
            {
                return await fce(CreateClient());
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new SuccessfulServiceCallResult<ApiException>(ex);
            }
        }

    }
}
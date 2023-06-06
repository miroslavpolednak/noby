namespace ExternalServices.ESignatures.V1;

internal sealed class MockESignaturesClient 
    : IESignaturesClient
{
    public Task DownloadDocumentPreview(string externalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetDocumentStatus(string documentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SubmitDispatchForm(bool documentsValid, List<Dto.DispatchFormClientDocument> documents, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

namespace ExternalServices.ESignatures.V1;

internal sealed class MockESignaturesClient 
    : IESignaturesClient
{
    public Task DownloadDocumentPreview(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetDocumentStatus(string documentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SubmitDispatchForm(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

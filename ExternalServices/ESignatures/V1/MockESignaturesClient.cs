using CIS.Foms.Types.Enums;

namespace ExternalServices.ESignatures.V1;

internal sealed class MockESignaturesClient 
    : IESignaturesClient
{
    public Task DeleteDocument(string externalId, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task<byte[]> DownloadDocumentPreview(string externalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<EDocumentStatuses> GetDocumentStatus(string documentId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(EDocumentStatuses.NEW);
    }

    public Task<long> PrepareDocument(Dto.PrepareDocumentRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<long>(1111);
    }

    public Task<(int? Code, string? Message)> SendDocumentPreview(string externalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SubmitDispatchForm(bool documentsValid, List<Dto.DispatchFormClientDocument> documents, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<(string ExternalId, string? TargetUrl)> UploadDocument(long referenceId, string filename, DateTime creationDate, byte[] fileData, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<(string, string?)>(("111", null));
    }
}

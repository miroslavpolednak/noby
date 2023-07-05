using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.ESignatures.V1;

public interface IESignaturesClient
    : IExternalServiceClient
{
    Task<string> GetDocumentStatus(string documentId, CancellationToken cancellationToken = default);

    Task DownloadDocumentPreview(string externalId, CancellationToken cancellationToken = default);

    Task SubmitDispatchForm(bool documentsValid, List<Dto.DispatchFormClientDocument> documents, CancellationToken cancellationToken = default);

    Task<(int? Code, string? Message)> SendDocumentPreview(string externalId, CancellationToken cancellationToken = default);

    const string Version = "V1";
}
using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.ESignatures.V1;

public interface IESignaturesClient
    : IExternalServiceClient
{
    Task<string> GetDocumentStatus(string documentId, CancellationToken cancellationToken = default);

    Task DownloadDocumentPreview(CancellationToken cancellationToken = default);

    Task SubmitDispatchForm(CancellationToken cancellationToken = default);

    const string Version = "V1";
}
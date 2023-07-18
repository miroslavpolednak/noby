using CIS.Foms.Types.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.ESignatures.V1;

public interface IESignaturesClient
    : IExternalServiceClient
{
    Task<EDocumentStatuses> GetDocumentStatus(string documentId, CancellationToken cancellationToken = default);

    Task DownloadDocumentPreview(string externalId, CancellationToken cancellationToken = default);

    Task SubmitDispatchForm(bool documentsValid, List<Dto.DispatchFormClientDocument> documents, CancellationToken cancellationToken = default);

    Task<(int? Code, string? Message)> SendDocumentPreview(string externalId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Předání metadat dokumentu do ePodpisů 
    /// </summary>
    /// <returns>ReferenceId, které se použije jako vstup pro metodu UploadDocument</returns>
    Task<long> PrepareDocument(Dto.PrepareDocumentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Předání souboru dokumentu do ePodpisu
    /// </summary>
    /// <param name="referenceId">ID metadat ke kterým má být dokument navázán (response z metody PrepareDocument)</param>
    /// <param name="filename">Název souboru dle Generování názvu PDF dokumentu</param>
    /// <param name="creationDate">DocumentOnSA.Created.CreatedOn</param>
    /// <returns>ExternalId - stejné jako vstupní externalId nebo nově vypočítané ePodpisy; TargetUrl - URL adresa</returns>
    Task<(string ExternalId, string? TargetUrl)> UploadDocument(
        long referenceId,
        string filename,
        DateTime creationDate,
        byte[] fileData,
        CancellationToken cancellationToken = default);

    const string Version = "V1";
}
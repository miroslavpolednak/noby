using DomainServices.DocumentArchiveService.Contracts;

namespace DomainServices.DocumentArchiveService.Clients;

public interface IDocumentArchiveServiceClient
{
    /// <summary>
    /// Metoda slouží k vygenerování ID dokumentu které se následně použije pro uložení dokumentu do eArchivu a pro registraci do ESCUDO.
    /// </summary>
    /// <returns><see cref="string"/> DocumentId</returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 14009; EnvironmentName hodnota není z enum (DEV, FAT, SIT, UAT, PREPROD, EDU, PROD)</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 14010; EnvironmentIndex není jednociferné číslo</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Service unavailable</exception>
    Task<string> GenerateDocumentId(GenerateDocumentIdRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k získání detailu dokumentu z eArchívu (CSP a TCP), jeho kompletních metadat a kopie samotného dokumentu (binární data - BLOB) na základě DocumentId
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 14003; Unable to get/find document(s) from eArchive (SCP/SDF).</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 14002; Unable to get/find document(s) from eArchive (TCP).</exception>
    Task<GetDocumentResponse> GetDocument(GetDocumentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k získání seznamu dokumentů (metadata) z eArchivu (CSP a TCP) na základě vstupních parametrů (např. CaseId, Autor, ...)
    /// </summary>
    Task<GetDocumentListResponse> GetDocumentList(GetDocumentListRequest request, CancellationToken cancellationToken = default);
}

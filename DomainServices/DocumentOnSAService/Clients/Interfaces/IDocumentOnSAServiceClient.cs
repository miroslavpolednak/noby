using DomainServices.DocumentOnSAService.Contracts;

namespace DomainServices.DocumentOnSAService.Clients;
public interface IDocumentOnSAServiceClient
{
    /// <summary>
    /// Metoda slouží k vygenerování FormId dle pravidel definovaných business analýzou. V aktuální verzi generujeme FormId jen s prefixem "N" protože Noby je aktuálně jediný konzument metody.
    /// </summary>
    Task<string> GenerateFormId(GenerateFormIdRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k zahájení podepisovacího procesu.
    /// </summary>
    Task<StartSigningResponse> StartSigning(StartSigningRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k přerušení podepisovacího procesu.
    /// </summary>
    Task StopSigning(int documentOnSAId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k poskytnutí seznamu dokumentů k podpisu.
    /// </summary>
    Task<GetDocumentsToSignListResponse> GetDocumentsToSignList(int salesArrangementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vrácení dat uložených v DocumentOnSA dle poskytnutého DocumentOnSAId, data lze pak použít k vygenerování PDF metodou generateDocument
    /// </summary>
    Task<GetDocumentOnSADataResponse> GetDocumentOnSAData(int documentOnSAId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k zahájení podepisovacího procesu
    /// </summary>
    Task SignDocument(int documentOnSAId, int signatureTypeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k poskytnutí seznamu všechn dokumentů.
    /// </summary>
    Task<GetDocumentsOnSAListResponse> GetDocumentsOnSAList(int salesArrangementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k vložení záznamu do tabulky DocumentOnSA obsahující zahájené podepisovací procesy 
    /// </summary>
    Task<CreateDocumentOnSAResponse> CreateDocumentOnSA(CreateDocumentOnSARequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k přidání eArchivového ID k Documentu na SA.
    /// </summary>
    Task LinkEArchivIdToDocumentOnSA(LinkEArchivIdToDocumentOnSARequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k vrácení PDF dokumentu z fronty ePodpisů.
    /// </summary>
    Task<GetElectronicDocumentFromQueueResponse> GetElectronicDocumentFromQueue(GetElectronicDocumentFromQueueRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k úpravě dat na DocumentOnSA
    /// </summary>
    Task SetDocumentOnSAArchived(int documentOnSAId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda vrací náhled elektronického dokumentu
    /// </summary>
    Task<GetElectronicDocumentPreviewResponse> GetElectronicDocumentPreview(int documentOnSAId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k odeslání náhledu dokumentu klientovi v případě elektronického podpisu
    /// </summary>
    Task SendDocumentPreview(int documentOnSAId, CancellationToken cancellationToken = default);
}

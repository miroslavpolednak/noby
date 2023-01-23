using DomainServices.DocumentOnSAService.Contracts;

namespace DomainServices.DocumentOnSAService.Clients;
public interface IDocumentOnSAServiceClient
{
    /// <summary>
    /// Metoda slouží k vygenerování FormId dle pravidel definovaných business analýzou. V aktuální verzi generujeme FormId jen s prefixem "N" protože Noby je aktuálně jediný konzument metody.
    /// </summary>
    Task<GenerateFormIdResponse> GenerateFormId(GenerateFormIdRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k zahájení podepisovacího procesu.
    /// </summary>
    Task<StartSigningResponse> StartSigning(StartSigningRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k přerušení podepisovacího procesu.
    /// </summary>
    Task StopSigning(StopSigningRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Metoda slouží k poskytnutí seznamu dokumentů k podpisu.
    /// </summary>
    Task<GetDocumentsToSignListResponse> GetDocumentsToSignList(GetDocumentsToSignListRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vrácení dat uložených v DocumentOnSA dle poskytnutého DocumentOnSAId, data lze pak použít k vygenerování PDF metodou generateDocument
    /// </summary>
    Task<GetDocumentOnSADataResponse> GetDocumentOnSAData(int documentOnSAId, CancellationToken cancellationToken = default);
}

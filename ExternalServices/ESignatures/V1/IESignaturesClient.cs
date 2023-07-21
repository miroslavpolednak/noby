using CIS.Foms.Types.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.ESignatures.V1;

public interface IESignaturesClient
    : IExternalServiceClient
{
    /// <summary>
    /// Dotaz na stav dokumentu.
    /// </summary>
    /// <remarks>
    /// Pokud API eP vrátí neznámý status (může se stát - např. v případě nenalezeního dokumentu), tak vracíme exception.
    /// </remarks>
    /// <param name="documentId">???</param>
    /// <exception cref="CisExtServiceResponseDeserializationException">Nepodařilo se deserializovat JSON response služby do očekávaného objektu</exception>
    /// <exception cref="CisExtServiceValidationException">Code=50001; Služba vrátila korektní result, nicméně Status není v našem enumu.</exception>
    /// <exception cref="CisExtServiceValidationException">Code=?; Služba vrátila chybový stav indikovaný vlastností Code. Exception má v tomto případě Code takový, jaký se vrátil z API, stejně tak jako Message.</exception>
    Task<EDocumentStatuses> GetDocumentStatus(string externalId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stažení náhledu úvodní verze dokumentu
    /// </summary>
    /// <param name="externalId"></param>
    /// <exception cref="CisExtServiceValidationException">Code=50005; Response neobsahuje binární data</exception>
    Task<byte[]> DownloadDocumentPreview(string externalId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finalizační průvodka
    /// </summary>
    /// <param name="documentsValid">Potvrzení, že zaslané dokumenty jsou čitelné</param>
    /// <param name="documents">Seznam externích ID dokumentů pro průvodku</param>
    /// <param name="newControllerRequest">Informace o změně kontrolera</param>
    /// <exception cref="CisExtServiceValidationException">Code=?; Služba vrátila chybový stav indikovaný vlastností Code. Exception má v tomto případě Code takový, jaký se vrátil z API, stejně tak jako Message.</exception>
    Task SubmitDispatchForm(bool documentsValid, List<Dto.DispatchFormClientDocument> documents, CancellationToken cancellationToken = default);

    Task<(int? Code, string? Message)> SendDocumentPreview(string externalId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Předání metadat dokumentu do ePodpisů 
    /// </summary>
    /// <returns>ReferenceId, které se použije jako vstup pro metodu UploadDocument</returns>
    /// <exception cref="CisExtServiceResponseDeserializationException">Nepodařilo se deserializovat JSON response služby do očekávaného objektu</exception>
    /// <exception cref="ArgumentNullException">Pokud je některý z povinných objektů null</exception>
    /// <exception cref="CisExtServiceValidationException">Code=50002,50003; Pokud některé z číselníkových ID nebylo v daném číselníku nalezeno</exception>
    /// <exception cref="CisExtServiceValidationException">Code=50004; ReferenceId not found in response</exception>
    /// <exception cref="CisExtServiceValidationException">Code=?; Služba vrátila chybový stav indikovaný vlastností Code. Exception má v tomto případě Code takový, jaký se vrátil z API, stejně tak jako Message.</exception>
    Task<long> PrepareDocument(Dto.PrepareDocumentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Předání souboru dokumentu do ePodpisu
    /// </summary>
    /// <param name="referenceId">ID metadat ke kterým má být dokument navázán (response z metody PrepareDocument)</param>
    /// <param name="filename">Název souboru dle Generování názvu PDF dokumentu</param>
    /// <param name="creationDate">DocumentOnSA.Created.CreatedOn</param>
    /// <returns>ExternalId - stejné jako vstupní externalId nebo nově vypočítané ePodpisy; TargetUrl - URL adresa</returns>
    /// <exception cref="CisExtServiceResponseDeserializationException">Nepodařilo se deserializovat JSON response služby do očekávaného objektu</exception>
    /// <exception cref="CisExtServiceValidationException">Code=?; Služba vrátila chybový stav indikovaný vlastností Code. Exception má v tomto případě Code takový, jaký se vrátil z API, stejně tak jako Message.</exception>
    Task<(string ExternalId, string? TargetUrl)> UploadDocument(
        long referenceId,
        string filename,
        DateTime creationDate,
        byte[] fileData,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Pokyn ke smazání dokumentu
    /// </summary>
    /// <param name="externalId"></param>
    /// <exception cref="CisExtServiceValidationException">Code=?; Služba vrátila chybový stav indikovaný vlastností Code. Exception má v tomto případě Code takový, jaký se vrátil z API, stejně tak jako Message.</exception>
    Task DeleteDocument(string externalId, CancellationToken cancellationToken = default);

    const string Version = "V1";
}
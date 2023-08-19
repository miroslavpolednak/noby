using CIS.Foms.Enums;
using NOBY.Dto.Signing;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSADetail;

public class GetDocumentOnSADetailResponse
{
    /// <summary>
    /// Id dokumentu. Vrací se pouze pro dokumenty se zahájeným podepisovacím procesem.
    /// </summary>
    public int? DocumentOnSAId { get; set; }

    /// <summary>
    /// Typ dokumentu. Číselník DocumentOnSAType.
    /// </summary>
    public int? DocumentTypeId { get; set; }

    /// <summary>
    /// Businessové modré ID
    /// </summary>
    public string? FormId { get; set; }

    /// <summary>
    /// Metoda podpisu (manuální/elektronický). Číselník SigningMethodsForNaturalPerson.
    /// </summary>
    public int? SignatureTypeId { get; set; }

    /// <summary>
    /// Timestamp, kdy došlo k podpisu celého dokumentu.
    /// </summary>
    public DateTime? SignatureDateTime { get; set; }

    public SignatureState SignatureState { get; set; } = null!;

    public EACodeMainItem EACodeMainItem { get; set; } = null!;

    public int? CustomerOnSAId { get; set; }

    /// <summary>
    /// Příznak, zda byl elektronicky podepisovaný dokument odeslán na klienta.
    /// </summary>
    public bool IsPreviewSentToCustomer { get; set; }

    /// <summary>
    /// Id elektronicky podepisovaného dokumentu nahraného v ePodpisech
    /// </summary>
    public string? ExternalId { get; set; }

    public Source Source { get; set; }
}


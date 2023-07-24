using CIS.Foms.Enums;

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
    public string? SignatureMethodCode { get; set; }

    /// <summary>
    /// Timestamp, kdy došlo k podpisu celého dokumentu.
    /// </summary>
    public DateTime? SignatureDateTime { get; set; }

    public SignatureState SignatureState { get; set; } = null!;

    public EACodeMainItem EACodeMainItem { get; set; } = null!;

    public int? CustomerOnSAId { get; set; }

    public bool IsPreviewSentToCustomer { get; set; }

    public string? ExternalId { get; set; }

    public Source Source { get; set; }
}

public class SignatureState
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public static implicit operator SignatureState(SignatureStateDto signatureStateDto)
    {
        return new SignatureState
        {
            Id = signatureStateDto.Id,
            Name = signatureStateDto.Name,
        };
    }
}

public class EACodeMainItem
{
    public int Id { get; set; }

    public string DocumentType { get; set; } = null!;

    public string Category { get; set; } = null!;

    public static implicit operator EACodeMainItem(EACodeMainItemDto eACode)
    {
        return new EACodeMainItem
        {
            Id = eACode.Id,
            DocumentType = eACode.DocumentType,
            Category = eACode.Category
        };
    }
}
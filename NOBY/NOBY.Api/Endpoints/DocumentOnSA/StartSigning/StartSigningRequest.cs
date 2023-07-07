using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.DocumentOnSA.StartSigning;

public class StartSigningRequest : IRequest<StartSigningResponse>
{
    [JsonIgnore]
    public int? SalesArrangementId { get; set; }

    /// <summary>
    /// Typ dokumentu. Číselník DocumentType.
    /// </summary>
    public int? DocumentTypeId { get; set; }

    /// <summary>
    /// Metoda podpisu (manuální/elektronický). Číselník SigningMethodsForNaturalPerson.
    /// </summary>
    [Obsolete("Replaced with SignatureTypeId")]
    public string SignatureMethodCode { get; set; } = null!;

    /// <summary>
    /// Metoda podpisu (manuální/elektronický). Číselník SignatureType.
    /// </summary>
    public int? SignatureTypeId { get; set; }

    public int? CustomerOnSAId1 { get; set; }

    public int? CustomerOnSAId2 { get; set; }

    internal StartSigningRequest InfuseSalesArrangementId(int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
        return this;
    }
}

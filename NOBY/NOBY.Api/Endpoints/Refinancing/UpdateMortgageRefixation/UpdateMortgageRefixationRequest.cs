using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Refinancing.UpdateMortgageRefixation;

public sealed class UpdateMortgageRefixationRequest
    : IRequest<NOBY.Dto.Refinancing.RefinancingLinkResult>
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    /// <summary>
    /// Komentář k Cenové výjimce
    /// </summary>
    /// <example>Prosím o slevu, jde o dlouhodobého loajálního klienta</example>
    public string? IndividualPriceCommentLastVersion { get; set; }

    /// <summary>
    /// Komentář k refixaci
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Sleva ze sazby
    /// </summary>
    /// <example>0.2</example>
    public decimal InterestRateDiscount { get; set; }

    internal UpdateMortgageRefixationRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}

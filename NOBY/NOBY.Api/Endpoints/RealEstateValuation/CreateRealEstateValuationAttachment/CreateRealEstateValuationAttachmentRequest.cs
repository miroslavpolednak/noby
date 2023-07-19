using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.RealEstateValuation.CreateRealEstateValuationAttachment;

public sealed class CreateRealEstateValuationAttachmentRequest
    : IRequest<int>
{
    [JsonIgnore]
    internal long CaseId;

    [JsonIgnore]
    internal int RealEstateValuationId;

    /// <summary>
    /// Popis souboru
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Soubor přílohy
    /// </summary>
    [Required]
    public IFormFile? File { get; set; }

    internal CreateRealEstateValuationAttachmentRequest InfuseId(long caseId, int realEstateValuationId)
    {
        this.CaseId = caseId;
        this.RealEstateValuationId = realEstateValuationId;
        return this;
    }
}

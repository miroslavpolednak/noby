using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement;

public sealed class CreateSalesArrangementRequest
    : IRequest<CreateSalesArrangementResponse>
{
    [JsonIgnore]
    internal long CaseId;

    /// <summary>
    /// Typ žádosti - číselník SalesArrangementType
    /// </summary>
    [Required]
    public int SalesArrangementTypeId { get; set; }

    internal CreateSalesArrangementRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}

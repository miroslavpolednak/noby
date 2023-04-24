using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement;

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

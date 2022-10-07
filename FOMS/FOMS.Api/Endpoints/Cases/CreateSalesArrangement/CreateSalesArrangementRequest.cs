using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FOMS.Api.Endpoints.Cases.CreateSalesArrangement;

public class CreateSalesArrangementRequest
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

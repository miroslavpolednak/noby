using System.Text.Json.Serialization;

namespace FOMS.Api.Endpoints.SalesArrangement.UpdateParameters;

public class UpdateParametersRequest
    : IRequest
{
    [JsonIgnore]
    internal int SalesArrangementId;

    /// <summary>
    /// Dalsi udaje o pripadu/uveru. Typ objektu je podle typu SA.
    /// </summary>
    /// <remarks>
    /// OneOf(
    /// DomainServices.SalesArrangementService.Contracts.SalesArrangementParametersMortgage
    /// )
    /// </remarks>
    public Dto.ParametersMortgage? Parameters { get; set; }

    internal UpdateParametersRequest InfuseId(int salesArrangementId)
    {
        this.SalesArrangementId = salesArrangementId;
        return this;
    }
}

using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

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
    /// NOBY.Api.Endpoints.SalesArrangement.Dto.ParametersMortgage
    /// NOBY.Api.Endpoints.SalesArrangement.Dto.ParametersDrawing
    /// )
    /// </remarks>
    public object? Parameters { get; set; }

    internal UpdateParametersRequest InfuseId(int salesArrangementId)
    {
        this.SalesArrangementId = salesArrangementId;
        return this;
    }
}

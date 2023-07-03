using System.Text.Json.Serialization;
using NOBY.Api.Endpoints.SalesArrangement.Dto;
using NOBY.Dto.Attributes;

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
    [SwaggerOneOf(typeof(ParametersMortgage),
                  typeof(ParametersDrawing),
                  typeof(Dto.HUBNUpdate),
                  typeof(Dto.GeneralChangeUpdate),
                  typeof(Dto.CustomerChangeUpdate),
                  typeof(Dto.CustomerChange3602Update))]
    public object? Parameters { get; set; }

    internal UpdateParametersRequest InfuseId(int salesArrangementId)
    {
        this.SalesArrangementId = salesArrangementId;
        return this;
    }
}

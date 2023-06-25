using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.RealEstateValuation;

[ApiController]
[Route("api/case")]
public sealed class RealEstateValuationController : ControllerBase
{
    private readonly IMediator _mediator;
    public RealEstateValuationController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{caseId:long}/real-estate-valuations")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task CreateRealEstateValuation([FromRoute] long caseId, [FromBody] CreateRealEstateValuation.CreateRealEstateValuationRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request.InfuseId(caseId), cancellationToken);

    [HttpGet("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}")]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task DeleteRealEstateValuation([FromRoute] long caseId, [FromRoute] int realEstateValuationId, CancellationToken cancellationToken)
        => await _mediator.Send(new DeleteRealEstateValuation.DeleteRealEstateValuationRequest(caseId, realEstateValuationId), cancellationToken);

    [HttpGet("{caseId:long}/real-estate-valuations")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task GetListRealEstateValuation([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetListRealEstateValuation.GetListRealEstateValuationRequest(caseId), cancellationToken);
}

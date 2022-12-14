using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.CustomerObligation;

[ApiController]
[Route("api/customer-on-sa")]
public class CustomerObligationController : ControllerBase
{
    /// <summary>
    /// Smazání závazku customera
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/DeleteObligation
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="obligationId">ID závazku ke smazani</param>
    [HttpDelete("{customerOnSAId:int}/obligation/{obligationId:int}")]
    [SwaggerOperation(Tags = new[] { "Klient - závazek" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task Delete([FromRoute] int customerOnSAId, [FromRoute] int obligationId)
        => await _mediator.Send(new DeleteObligation.DeleteObligationRequest(customerOnSAId, obligationId));

    /// <summary>
    /// Detail závazku customera
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/GetObligation
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="obligationId">ID závazku</param>
    /// <returns>
    /// <see cref="Dto.ObligationFullDto"/>
    /// </returns>
    [HttpGet("{customerOnSAId:int}/obligation/{obligationId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient - závazek" })]
    [ProducesResponseType(typeof(Dto.ObligationFullDto), StatusCodes.Status200OK)]
    public async Task<Dto.ObligationFullDto> GetDetail([FromRoute] int customerOnSAId, [FromRoute] int obligationId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetObligation.GetObligationRequest(customerOnSAId, obligationId), cancellationToken);

    /// <summary>
    /// Update detailu závazku customera
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/UpdateObligation
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="obligationId">ID závazku</param>
    [HttpPut("{customerOnSAId:int}/obligation/{obligationId:int}")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient - závazek" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task Update([FromRoute] int customerOnSAId, [FromRoute] int obligationId, [FromBody] UpdateObligation.UpdateObligationRequest? request)
        => await _mediator.Send(request?.InfuseId(customerOnSAId, obligationId) ?? throw new CisArgumentNullException(ErrorCodes.PayloadIsEmpty, "Payload is empty", nameof(request)));

    /// <summary>
    /// Vytvoření závazku customera
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/CreateObligation
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    [HttpPost("{customerOnSAId:int}/obligation")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient - závazek" })]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<int> Create([FromRoute] int customerOnSAId, [FromBody] CreateObligation.CreateObligationRequest? request)
        => await _mediator.Send(request?.InfuseId(customerOnSAId) ?? throw new CisArgumentNullException(ErrorCodes.PayloadIsEmpty, "Payload is empty", nameof(request)));

    private readonly IMediator _mediator;
    public CustomerObligationController(IMediator mediator) => _mediator = mediator;
}

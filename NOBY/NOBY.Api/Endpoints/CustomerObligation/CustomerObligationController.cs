using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.CustomerObligation;

[ApiController]
[Route("api/customer-on-sa")]
[ApiVersion(1)]
public class CustomerObligationController : ControllerBase
{
    /// <summary>
    /// Smazání závazku customera
    /// </summary>
    /// <remarks>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=A3325947-AFC4-444a-989E-1531C4AFFEDE"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="obligationId">ID závazku ke smazani</param>
    [HttpDelete("{customerOnSAId:int}/obligation/{obligationId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = new[] { "Klient - závazek" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task DeleteObligation([FromRoute] int customerOnSAId, [FromRoute] int obligationId)
        => await _mediator.Send(new DeleteObligation.DeleteObligationRequest(customerOnSAId, obligationId));

    /// <summary>
    /// Detail závazku customera
    /// </summary>
    /// <remarks>
    ///  <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=B816782F-3F9B-4709-8D90-CAD6DB020F70"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="obligationId">ID závazku</param>
    /// <returns>
    /// <see cref="Dto.ObligationFullDto"/>
    /// </returns>
    [HttpGet("{customerOnSAId:int}/obligation/{obligationId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient - závazek" })]
    [ProducesResponseType(typeof(Dto.ObligationFullDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Dto.ObligationFullDto> GetObligation([FromRoute] int customerOnSAId, [FromRoute] int obligationId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetObligation.GetObligationRequest(customerOnSAId, obligationId), cancellationToken);

    /// <summary>
    /// Update detailu závazku customera
    /// </summary>
    /// <remarks>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=9AA4E44C-58B8-44c5-897A-64857ADD69DE"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="obligationId">ID závazku</param>
    [HttpPut("{customerOnSAId:int}/obligation/{obligationId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient - závazek" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateObligation([FromRoute] int customerOnSAId, [FromRoute] int obligationId, [FromBody] UpdateObligation.UpdateObligationRequest? request)
        => await _mediator.Send(request?.InfuseId(customerOnSAId, obligationId) ?? throw new NobyValidationException("Payload is empty"));

    /// <summary>
    /// Vytvoření závazku customera
    /// </summary>
    /// <remarks>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=15CBD05C-8B14-43a9-918F-5BAB59148D4D"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    [HttpPost("{customerOnSAId:int}/obligation")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient - závazek" })]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<int> CreateObligation([FromRoute] int customerOnSAId, [FromBody] CreateObligation.CreateObligationRequest? request)
        => await _mediator.Send(request?.InfuseId(customerOnSAId) ?? throw new NobyValidationException("Payload is empty"));

    private readonly IMediator _mediator;
    public CustomerObligationController(IMediator mediator) => _mediator = mediator;
}

using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.CustomerObligation;

[ApiController]
[Route("api/v{v:apiVersion}/customer-on-sa")]
[ApiVersion(1)]
public class CustomerObligationController(IMediator _mediator) 
    : ControllerBase
{
    /// <summary>
    /// Smazání závazku customera
    /// </summary>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="obligationId">ID závazku ke smazani</param>
    [HttpDelete("{customerOnSAId:int}/obligation/{obligationId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Klient - závazek"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=A3325947-AFC4-444a-989E-1531C4AFFEDE")]
    public async Task<IActionResult> DeleteObligation([FromRoute] int customerOnSAId, [FromRoute] int obligationId)
    {
        await _mediator.Send(new DeleteObligation.DeleteObligationRequest(customerOnSAId, obligationId));
        return NoContent();
    }
        

    /// <summary>
    /// Detail závazku customera
    /// </summary>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="obligationId">ID závazku</param>
    [HttpGet("{customerOnSAId:int}/obligation/{obligationId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient - závazek"])]
    [ProducesResponseType(typeof(CustomerObligationObligationFull), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=B816782F-3F9B-4709-8D90-CAD6DB020F70")]
    public async Task<CustomerObligationObligationFull> GetObligation([FromRoute] int customerOnSAId, [FromRoute] int obligationId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetObligation.GetObligationRequest(customerOnSAId, obligationId), cancellationToken);

    /// <summary>
    /// Update detailu závazku customera
    /// </summary>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="obligationId">ID závazku</param>
    [HttpPut("{customerOnSAId:int}/obligation/{obligationId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient - závazek"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=9AA4E44C-58B8-44c5-897A-64857ADD69DE")]
    public async Task<IActionResult> UpdateObligation([FromRoute] int customerOnSAId, [FromRoute] int obligationId, [FromBody] CustomerObligationUpdateObligationRequest? request)
    {
        await _mediator.Send(request?.InfuseId(customerOnSAId, obligationId) ?? throw new NobyValidationException("Payload is empty"));
        return NoContent();
    }
        

    /// <summary>
    /// Vytvoření závazku customera
    /// </summary>
    /// <param name="customerOnSAId">ID customera</param>
    [HttpPost("{customerOnSAId:int}/obligation")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient - závazek"])]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=15CBD05C-8B14-43a9-918F-5BAB59148D4D")]
    public async Task<int> CreateObligation([FromRoute] int customerOnSAId, [FromBody] CustomerObligationCreateObligationRequest? request)
        => await _mediator.Send(request?.InfuseId(customerOnSAId) ?? throw new NobyValidationException("Payload is empty"));
}

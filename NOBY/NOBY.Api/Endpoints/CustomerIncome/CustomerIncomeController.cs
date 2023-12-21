using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.CustomerIncome;

[ApiController]
[Route("api/customer-on-sa")]
[ApiVersion(1)]
public class CustomerIncomeController : ControllerBase
{
    /// <summary>
    /// Smazání příjmu customera
    /// </summary>
    /// <remarks>
    /// Ověří, že příjem patří customerovi a příjem smaže.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=91635463-1E2E-4dc5-B427-72C528298C88"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID příjmu ke smazání</param>
    [HttpDelete("{customerOnSAId:int}/income/{incomeId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = new[] { "Klient - příjem" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task DeleteIncome([FromRoute] int customerOnSAId, [FromRoute] int incomeId)
        => await _mediator.Send(new DeleteIncome.DeleteIncomeRequest(customerOnSAId, incomeId));

    /// <summary>
    /// Detail příjmu customera
    /// </summary>
    /// <remarks>
    /// <strong>CustomerIncome.GetDetail</strong><br/>
    /// Ověří, že příjem patří customerovi a příjem smaže.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=91635463-1E2E-4dc5-B427-72C528298C88"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID příjmu</param>
    /// <returns>
    /// <see cref="Dto.IncomeDataEmployement"/>
    /// </returns>
    [HttpGet("{customerOnSAId:int}/income/{incomeId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient - příjem" })]
    [ProducesResponseType(typeof(GetIncome.GetIncomeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetIncome.GetIncomeResponse> GetIncome([FromRoute] int customerOnSAId, [FromRoute] int incomeId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetIncome.GetIncomeRequest(customerOnSAId, incomeId), cancellationToken);

    /// <summary>
    /// Update detailu příjmu customera
    /// </summary>
    /// <remarks>
    /// Ověří, že příjem patří customerovi a příjem smaže.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=91635463-1E2E-4dc5-B427-72C528298C88"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a> 
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID příjmu</param>
    [HttpPut("{customerOnSAId:int}/income/{incomeId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient - příjem" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateIncome([FromRoute] int customerOnSAId, [FromRoute] int incomeId, [FromBody] UpdateIncome.UpdateIncomeRequest? request)
        => await _mediator.Send(request?.InfuseId(customerOnSAId, incomeId) ?? throw new NobyValidationException("Payload is empty"));

    /// <summary>
    /// Vytvoření příjmu customera
    /// </summary>
    /// <remarks>
    /// Vytvoří příjem k danému customerovi.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=388BC673-9722-45e3-88B3-838E85C99912"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    [HttpPost("{customerOnSAId:int}/income")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient - příjem" })]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<int> CreateIncome([FromRoute] int customerOnSAId, [FromBody] CreateIncome.CreateIncomeRequest? request)
        => await _mediator.Send(request?.InfuseId(customerOnSAId) ?? throw new NobyValidationException("Payload is empty"));

    private readonly IMediator _mediator;
    public CustomerIncomeController(IMediator mediator) => _mediator = mediator;
}

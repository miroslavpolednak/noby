using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.CustomerIncome;

[ApiController]
[Route("api/v{v:apiVersion}/customer-on-sa")]
[ApiVersion(1)]
public class CustomerIncomeController(IMediator _mediator) 
    : ControllerBase
{
    /// <summary>
    /// Smazání příjmu customera
    /// </summary>
    /// <remarks>
    /// Ověří, že příjem patří customerovi a příjem smaže.
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID příjmu ke smazání</param>
    [HttpDelete("{customerOnSAId:int}/income/{incomeId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Klient - příjem"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=91635463-1E2E-4dc5-B427-72C528298C88")]
    public async Task<IActionResult> DeleteIncome([FromRoute] int customerOnSAId, [FromRoute] int incomeId)
    {
        await _mediator.Send(new DeleteIncome.DeleteIncomeRequest(customerOnSAId, incomeId));
        return NoContent();
    }
        

    /// <summary>
    /// Detail příjmu customera
    /// </summary>
    /// <remarks>
    /// <strong>CustomerIncome.GetDetail</strong><br/><br/>Použít pro zobrazení detailu příjmu - tj. Level 2 obrazovka prokliknutá z detailu domacnosti.
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID příjmu</param>
    [HttpGet("{customerOnSAId:int}/income/{incomeId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient - příjem"])]
    [ProducesResponseType(typeof(CustomerIncomeGetIncomeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=B796BF15-1B08-4ac7-9CCE-D5E1BB5A35A3")]
    public async Task<CustomerIncomeGetIncomeResponse> GetIncome([FromRoute] int customerOnSAId, [FromRoute] int incomeId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetIncome.GetIncomeRequest(customerOnSAId, incomeId), cancellationToken);

    /// <summary>
    /// Update detailu příjmu customera
    /// </summary>
    /// <remarks>
    /// <strong>CustomerIncome.Update</strong><br/><br/>Použít pro update detailu příjmu - tj. Level 2 obrazovka prokliknutá z detailu domácnosti.
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID příjmu</param>
    [HttpPut("{customerOnSAId:int}/income/{incomeId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient - příjem"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=FAA06BC2-E216-42fe-9EA7-630528373F92")]
    public async Task<IActionResult> UpdateIncome([FromRoute] int customerOnSAId, [FromRoute] int incomeId, [FromBody] CustomerIncomeUpdateIncomeRequest? request)
    {
        await _mediator.Send(request?.InfuseId(customerOnSAId, incomeId) ?? throw new NobyValidationException("Payload is empty"));
        return NoContent();
    }
        

    /// <summary>
    /// Vytvoření příjmu customera
    /// </summary>
    /// <remarks>
    /// Vytvoří příjem k danému customerovi.
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    [HttpPost("{customerOnSAId:int}/income")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient - příjem"])]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=388BC673-9722-45e3-88B3-838E85C99912")]
    public async Task<int> CreateIncome([FromRoute] int customerOnSAId, [FromBody] CustomerIncomeCreateIncomeRequest? request)
        => await _mediator.Send(request?.InfuseId(customerOnSAId) ?? throw new NobyValidationException("Payload is empty"));
}

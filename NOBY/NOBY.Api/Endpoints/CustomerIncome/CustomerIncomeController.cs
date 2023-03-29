using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.CustomerIncome;

[ApiController]
[Route("api/customer-on-sa")]
public class CustomerIncomeController : ControllerBase
{
    /// <summary>
    /// Smazání příjmu customera
    /// </summary>
    /// <remarks>
    /// <strong>CustomerIncome.Delete</strong><br/>
    /// Tento endpoint se v současné době asi používat nebude.<br/>
    /// <i>DS:</i> SalesArrangementService/DeleteIncome
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID příjmu ke smazání</param>
    [HttpDelete("{customerOnSAId:int}/income/{incomeId:int}")]
    [SwaggerOperation(Tags = new[] { "Klient - příjem" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task Delete([FromRoute] int customerOnSAId, [FromRoute] int incomeId)
        => await _mediator.Send(new DeleteIncome.DeleteIncomeRequest(customerOnSAId, incomeId));

    /// <summary>
    /// Detail příjmu customera
    /// </summary>
    /// <remarks>
    /// <strong>CustomerIncome.GetDetail</strong><br/>
    /// Použít pro zobrazení detailu příjmu - tj. Level 2 obrazovka prokliknutá z detailu domacnosti.<br/>
    /// <i>DS:</i> SalesArrangementService/GetIncome
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID příjmu</param>
    /// <returns>
    /// <see cref="Dto.IncomeDataEmployement"/>
    /// </returns>
    [HttpGet("{customerOnSAId:int}/income/{incomeId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient - příjem" })]
    [ProducesResponseType(typeof(GetIncome.GetIncomeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetIncome.GetIncomeResponse> GetDetail([FromRoute] int customerOnSAId, [FromRoute] int incomeId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetIncome.GetIncomeRequest(customerOnSAId, incomeId), cancellationToken);

    /// <summary>
    /// Update detailu příjmu customera
    /// </summary>
    /// <remarks>
    /// <strong>CustomerIncome.Update</strong><br/>
    /// Použít pro update detailu příjmu - tj. Level 2 obrazovka prokliknutá z detailu domácnosti.<br/>
    /// <i>DS:</i> SalesArrangementService/UpdateIncome
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID příjmu</param>
    [HttpPut("{customerOnSAId:int}/income/{incomeId:int}")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient - příjem" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task Update([FromRoute] int customerOnSAId, [FromRoute] int incomeId, [FromBody] UpdateIncome.UpdateIncomeRequest? request)
        => await _mediator.Send(request?.InfuseId(customerOnSAId, incomeId) ?? throw new NobyValidationException("Payload is empty"));

    /// <summary>
    /// Vytvoření příjmu customera
    /// </summary>
    /// <remarks>
    /// <strong>CustomerIncome.Create</strong><br/>
    /// <i>DS:</i> SalesArrangementService/CreateIncome
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    [HttpPost("{customerOnSAId:int}/income")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient - příjem" })]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<int> Create([FromRoute] int customerOnSAId, [FromBody] CreateIncome.CreateIncomeRequest? request)
        => await _mediator.Send(request?.InfuseId(customerOnSAId) ?? throw new NobyValidationException("Payload is empty"));

    private readonly IMediator _mediator;
    public CustomerIncomeController(IMediator mediator) => _mediator = mediator;
}

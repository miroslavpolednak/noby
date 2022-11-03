using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.CustomerIncome;

[ApiController]
[Route("api/customer-on-sa")]
public class CustomerIncomeController : ControllerBase
{
    /// <summary>
    /// Smazani prijmu customera
    /// </summary>
    /// <remarks>
    /// <strong>CustomerIncome.Delete</strong><br/>
    /// Tento endpoint se v soucasne dobe asi pouzivat nebude.<br/>
    /// <i>DS:</i> SalesArrangementService/DeleteIncome
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID prijmu ke smazani</param>
    [HttpDelete("{customerOnSAId:int}/income/{incomeId:int}")]
    [SwaggerOperation(Tags = new[] { "UC: Prijem" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task Delete([FromRoute] int customerOnSAId, [FromRoute] int incomeId)
        => await _mediator.Send(new DeleteIncome.DeleteIncomeRequest(customerOnSAId, incomeId));

    /// <summary>
    /// Detail prijmu customera
    /// </summary>
    /// <remarks>
    /// <strong>CustomerIncome.GetDetail</strong><br/>
    /// Pouzit pro zobrazeni detailu prijmu - tj. Level 2 obrazovka prokliknuta z detailu domacnosti.<br/>
    /// <i>DS:</i> SalesArrangementService/GetIncome
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID prijmu</param>
    /// <returns>
    /// <see cref="Dto.IncomeDataEmployement"/>
    /// </returns>
    [HttpGet("{customerOnSAId:int}/income/{incomeId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Prijem" })]
    [ProducesResponseType(typeof(GetIncome.GetIncomeResponse), StatusCodes.Status200OK)]
    public async Task<GetIncome.GetIncomeResponse> GetDetail([FromRoute] int customerOnSAId, [FromRoute] int incomeId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetIncome.GetIncomeRequest(customerOnSAId, incomeId), cancellationToken);

    /// <summary>
    /// Update detailu prijmu customera
    /// </summary>
    /// <remarks>
    /// <strong>CustomerIncome.Update</strong><br/>
    /// Pouzit pro update detailu prijmu - tj. Level 2 obrazovka prokliknuta z detailu domacnosti.<br/>
    /// <i>DS:</i> SalesArrangementService/UpdateIncome
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID prijmu</param>
    [HttpPut("{customerOnSAId:int}/income/{incomeId:int}")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Prijem" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task Update([FromRoute] int customerOnSAId, [FromRoute] int incomeId, [FromBody] UpdateIncome.UpdateIncomeRequest? request)
        => await _mediator.Send(request?.InfuseId(customerOnSAId, incomeId) ?? throw new CisArgumentNullException(ErrorCodes.PayloadIsEmpty, "Payload is empty", nameof(request)));

    /// <summary>
    /// Vytvoreni prijmu customera
    /// </summary>
    /// <remarks>
    /// <strong>CustomerIncome.Create</strong><br/>
    /// <i>DS:</i> SalesArrangementService/CreateIncome
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    [HttpPost("{customerOnSAId:int}/income")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Prijem" })]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<int> Create([FromRoute] int customerOnSAId, [FromBody] CreateIncome.CreateIncomeRequest? request)
        => await _mediator.Send(request?.InfuseId(customerOnSAId) ?? throw new CisArgumentNullException(ErrorCodes.PayloadIsEmpty, "Payload is empty", nameof(request)));

    private readonly IMediator _mediator;
    public CustomerIncomeController(IMediator mediator) => _mediator = mediator;
}

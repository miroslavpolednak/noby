using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.CustomerIncome;

[ApiController]
[Route("api/customer-on-sa")]
public class CustomerIncomeController : ControllerBase
{
    /// <summary>
    /// Seznam prijmu pro daneho customera
    /// </summary>
    /// <remarks>
    /// Vraci zakladni seznam prijmu bez detailu - pouze spolecne vlastnosti.<br/>
    /// <i>DS:</i> SalesArrangementService/GetIncomeList
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <returns>Seznam prijmu</returns>
    [HttpGet("{customerOnSAId:int}/income")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Domacnost" })]
    [ProducesResponseType(typeof(List<GetIncomes.IncomeInList>), StatusCodes.Status200OK)]
    public async Task<List<GetIncomes.IncomeInList>> GetList([FromRoute] int customerOnSAId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetIncomes.GetIncomesRequest(customerOnSAId), cancellationToken);

    /// <summary>
    /// Smazani prijmu customera
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/DeleteIncome
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID prijmu ke smazani</param>
    /// <returns>ID smazaneho prijmu</returns>
    [HttpDelete("{customerOnSAId:int}/income/{incomeId:int}")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Domacnost" })]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<int> Delete([FromRoute] int customerOnSAId, [FromRoute] int incomeId, CancellationToken cancellationToken)
        => await _mediator.Send(new DeleteIncome.DeleteIncomeRequest(customerOnSAId, incomeId), cancellationToken);

    /// <summary>
    /// Vytvoreni noveho prijmu customera
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/CreateIncome
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <returns>ID noveho prijmu</returns>
    [HttpPost("{customerOnSAId:int}/income")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Domacnost" })]
    [ProducesResponseType(typeof(int[]), StatusCodes.Status200OK)]
    public async Task<int[]> CreateIncomes([FromRoute] int customerOnSAId, [FromBody] CreateIncomes.CreateIncomesRequest? request, CancellationToken cancellationToken)
        => await _mediator.Send(request?.InfuseId(customerOnSAId) ?? throw new CisArgumentNullException(0, "Payload is empty", nameof(request)), cancellationToken);

    /// <summary>
    /// Update detailu prijmu customera
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/UpdateIncome
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID prijmu</param>
    /// <returns>ID prijmu</returns>
    [HttpPut("{customerOnSAId:int}/income/{incomeId:int}")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Domacnost" })]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<int> Update([FromRoute] int customerOnSAId, [FromRoute] int incomeId, [FromBody] UpdateIncome.UpdateIncomeRequest? request, CancellationToken cancellationToken)
        => await _mediator.Send(request?.InfuseId(customerOnSAId, incomeId) ?? throw new CisArgumentNullException(0, "Payload is empty", nameof(request)), cancellationToken);

    private readonly IMediator _mediator;
    public CustomerIncomeController(IMediator mediator) => _mediator = mediator;
}

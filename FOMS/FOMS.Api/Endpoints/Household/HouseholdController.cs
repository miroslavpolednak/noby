using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.Household;

/// <summary>
/// Prace s domacnostmi a customery
/// </summary>
[ApiController]
[Route("api/household")]
public class HouseholdController : ControllerBase
{
    /// <summary>
    /// Seznam domacnosti
    /// </summary>
    /// <remarks>
    /// Vraci zakladni seznam domacnosti pro dany Sales Arrangement bez detailu.<br/>
    /// <i>DS:</i> SalesArrangementService/GetHouseholdList
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement-u</param>
    /// <returns>Seznam domacnosti pro aktualni Sales Arrangement</returns>
    [HttpGet("list/{salesArrangementId:long}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Domacnost" })]
    [ProducesResponseType(typeof(GetHouseholds.GetHouseholdsResponse), StatusCodes.Status200OK)]
    public async Task<GetHouseholds.GetHouseholdsResponse> GetList([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetHouseholds.GetHouseholdsRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Detail domacnosti
    /// </summary>
    /// <remarks>
    /// Vraci detail dane domacnosti vcetne detailu navazanych CustomerOnSA.<br/>
    /// <i>DS:</i> SalesArrangementService/GetHousehold
    /// </remarks>
    /// <param name="householdId">ID domacnosti</param>
    /// <returns>Detail domacnosti</returns>
    [HttpGet("{householdId:long}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Domacnost" })]
    [ProducesResponseType(typeof(GetHousehold.GetHouseholdResponse), StatusCodes.Status200OK)]
    public async Task<GetHousehold.GetHouseholdResponse> GetHousehold([FromRoute] int householdId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetHousehold.GetHouseholdRequest(householdId), cancellationToken);

    /// <summary>
    /// Smazani domacnosti
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/DeleteHousehold
    /// </remarks>
    /// <param name="householdId">ID domacnosti</param>
    /// <returns>ID smazane domacnosti</returns>
    [HttpDelete("{householdId:int}")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Domacnost" })]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<int> Delete([FromRoute] int householdId, CancellationToken cancellationToken)
        => await _mediator.Send(new DeleteHousehold.DeleteHouseholdRequest(householdId), cancellationToken);

    /// <summary>
    /// Vytvoreni nove domacnosti
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/CreateHousehold<br/>
    /// <i>DS:</i> SalesArrangementService/CreateCustomer
    /// </remarks>
    /// <returns>Nove HouseholdId, typ domacnosti a nazev typu domacnosti</returns>
    [HttpPost("")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Domacnost" })]
    [ProducesResponseType(typeof(Dto.HouseholdInList), StatusCodes.Status200OK)]
    public async Task<Dto.HouseholdInList> Create([FromBody] CreateHousehold.CreateHouseholdRequest? request, CancellationToken cancellationToken)
        => await _mediator.Send(request ?? throw new CisArgumentNullException(0, "Payload is empty", nameof(request)), cancellationToken);

    /// <summary>
    /// Update existujici domacnosti
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/UpdateHousehold
    /// </remarks>
    /// <param name="householdId">ID domacnosti</param>
    /// <returns>HouseholdId</returns>
    [HttpPut("{householdId:int}")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Domacnost" })]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<int> Update([FromRoute] int householdId, [FromBody] UpdateHousehold.UpdateHouseholdRequest? request, CancellationToken cancellationToken)
        => await _mediator.Send(request?.InfuseId(householdId) ?? throw new CisArgumentNullException(0, "Payload is empty", nameof(request)), cancellationToken);

    /// <summary>
    /// Update customeru na domacnosti
    /// </summary>
    /// <param name="householdId">ID domacnosti</param>
    /// <returns>CustomerOnSAId nalinkovanych customeru</returns>
    [HttpPut("{householdId:int}/customers")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Domacnost" })]
    [ProducesResponseType(typeof(UpdateCustomers.UpdateCustomersResponse), StatusCodes.Status200OK)]
    public async Task<UpdateCustomers.UpdateCustomersResponse> UpdateCustomers([FromRoute] int householdId, [FromBody] UpdateCustomers.UpdateCustomersRequest? request, CancellationToken cancellationToken)
        => await _mediator.Send(request?.InfuseId(householdId) ?? throw new CisArgumentNullException(0, "Payload is empty", nameof(request)), cancellationToken);

    private readonly IMediator _mediator;
    public HouseholdController(IMediator mediator) =>  _mediator = mediator;
}
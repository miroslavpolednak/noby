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
    /// [DEV] Seznam domacnosti
    /// </summary>
    /// <remarks>
    /// DS: SalesArrangementService/GetHouseholdList
    /// DS: SalesArrangementService/GetCustomerList
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement-u</param>
    /// <returns>Seznam domacnosti pro aktualni Sales Arrangement vcetne navazanych customeru</returns>
    [HttpGet("list/{salesArrangementId:long}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Domacnost" })]
    [ProducesResponseType(typeof(List<Dto.Household>), StatusCodes.Status200OK)]
    public async Task<List<Dto.Household>> GetList([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetHouseholds.GetHouseholdsRequest(salesArrangementId), cancellationToken);
    
    /// <summary>
    /// [DEV] Ulozeni domacnosti
    /// </summary>
    /// <remarks>
    /// DS: SalesArrangementService/UpdateHousehold
    /// DS: SalesArrangementService/CreateHousehold
    /// DS: SalesArrangementService/UpdateCustomer
    /// DS: SalesArrangementService/CreateCustomer
    /// </remarks>
    /// <returns>Seznam ulozenych/nove vytvorenych HouseholdId</returns>
    [HttpPost("list")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Domacnost" })]
    [ProducesResponseType(typeof(List<int>), StatusCodes.Status200OK)]
    public async Task<List<int>> Save([FromBody] SaveHouseholds.SaveHouseholdsRequest? request, CancellationToken cancellationToken)
        => await _mediator.Send(request ?? throw new CisArgumentNullException(0, "Payload is empty", nameof(request)), cancellationToken);
    
    private readonly IMediator _mediator;
    public HouseholdController(IMediator mediator) =>  _mediator = mediator;
}
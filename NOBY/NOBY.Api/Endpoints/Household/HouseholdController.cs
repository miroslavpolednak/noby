using DomainServices.SalesArrangementService.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using System.Drawing.Drawing2D;

namespace NOBY.Api.Endpoints.Household;

/// <summary>
/// Prace s domacnostmi a customery
/// </summary>
[ApiController]
[Route("api/household")]
public class HouseholdController : ControllerBase
{
    /// <summary>
    /// Seznam domácností
    /// </summary>
    /// <remarks>
    /// Vraci zakladni seznam domacností pro daný Sales Arrangement bez detailu.<br/>
    /// <i>DS:</i> SalesArrangementService/GetHouseholdList
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement-u</param>
    /// <returns><see cref="List{T}"/> where T : <see cref="Dto.HouseholdInList"/> Seznam domacnosti pro dany Sales Arrangement</returns>
    [HttpGet("list/{salesArrangementId:long}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "Domácnost" })]
    [ProducesResponseType(typeof(List<Dto.HouseholdInList>), StatusCodes.Status200OK)]
    public async Task<List<Dto.HouseholdInList>> GetList([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetHouseholds.GetHouseholdsRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Detail domacnosti
    /// </summary>
    /// <remarks>
    /// Vrací detail dané domácnosti včetně detailu navázaných CustomerOnSA. Příznak druh/družka (areCustomersPartners) je počítán dynamicky při každém provolání dle logiky ve společných algoritmech.
    /// <i>DS:</i> SalesArrangementService/GetHousehold
    /// </remarks>
    /// <param name="householdId">ID domacnosti</param>
    /// <returns><see cref="GetHousehold.GetHouseholdResponse"/> Detail domacnosti</returns>
    [HttpGet("{householdId:long}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Domácnost" })]
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
    /// <returns>ID smazané domacnosti</returns>
    [HttpDelete("{householdId:int}")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Domácnost" })]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<int> Delete([FromRoute] int householdId)
        => await _mediator.Send(new DeleteHousehold.DeleteHouseholdRequest(householdId));

    /// <summary>
    /// Vytvořeni nové domacnosti
    /// </summary>
    /// <remarks>
    /// Vytvoří nový household zavoláním DS: SalesArrangementService/CreateHousehold a zároveň vytvoří prvního CustomerOnSA s rolí odpovídající vytvářené domácnosti (Spolužadatel na spolužadatelské a ručitel na ručitelské domácnosti) pomocí volání DS: SalesArrangementService/CreateCustomer.<br/>
    /// Prázdná obálka CustomerOnSA je vytvářena, protože neexistuje businessově prázdná domácnost bez členů domácnosti.
    /// </remarks>
    /// <returns>Nove HouseholdId, typ domacnosti a nazev typu domacnosti</returns>
    [HttpPost("")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Domácnost" })]
    [ProducesResponseType(typeof(Dto.HouseholdInList), StatusCodes.Status200OK)]
    public async Task<Dto.HouseholdInList> Create([FromBody] CreateHousehold.CreateHouseholdRequest? request)
        => await _mediator.Send(request ?? throw new CisArgumentNullException(ErrorCodes.PayloadIsEmpty, "Payload is empty", nameof(request)));

    /// <summary>
    /// Update existující domácnosti
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/UpdateHousehold
    /// </remarks>
    /// <param name="householdId">ID domácnosti</param>
    [HttpPut("{householdId:int}")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Domácnost" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task Update([FromRoute] int householdId, [FromBody] UpdateHousehold.UpdateHouseholdRequest? request)
        => await _mediator.Send(request?.InfuseId(householdId) ?? throw new CisArgumentNullException(ErrorCodes.PayloadIsEmpty, "Payload is empty", nameof(request)));

    /// <summary>
    /// Update customeru na domácnosti
    /// </summary>
    /// <remarks>
    /// Založí, případně updatuje CustomerOnSAId1 a CustomerOnSAId2 podle objektu v requestu Customer1 a Customer2.
    /// <ul>
    /// <li>Customer1 = NULL; pokud je v puvodni domacnosti v CustomerOnSAId1 existujici ID, je toto ID smazano, vcetne entity CustomerOnSA</li>
    /// <li>Customer1.CustomerOnSAId = NULL; vola se CustomerOnSAService.CreateCustomer; pote se doplni Household.CustomerOnSAId1 nove vytvorenym ID</li>
    /// <li>Customer1.CustomerOnSAId = existujici customer; vola se CustomerOnSAService.UpdateCustomer</li>
    /// </ul>
    /// Pokud je pri update nebo create klient identifikovan, rezervuje se mu modre ID a pousti se flow identifikovaneho klienta.<br/>
    /// při opakovaném zavolání API s locked income = true nedochází k přepisování timestamp
    /// </remarks>
    /// <param name="householdId">ID domácnosti</param>
    /// <returns>CustomerOnSAId nalinkovanych customeru</returns>
    [HttpPut("{householdId:int}/customers")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Domácnost" })]
    [ProducesResponseType(typeof(UpdateCustomers.UpdateCustomersResponse), StatusCodes.Status200OK)]
    public async Task<UpdateCustomers.UpdateCustomersResponse> UpdateCustomers([FromRoute] int householdId, [FromBody] UpdateCustomers.UpdateCustomersRequest? request)
        => await _mediator.Send(request?.InfuseId(householdId) ?? throw new CisArgumentNullException(ErrorCodes.PayloadIsEmpty, "Payload is empty", nameof(request)));

    private readonly IMediator _mediator;
    public HouseholdController(IMediator mediator) =>  _mediator = mediator;
}
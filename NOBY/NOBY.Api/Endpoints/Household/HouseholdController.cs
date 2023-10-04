using DomainServices.SalesArrangementService.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Household;

/// <summary>
/// Prace s domacnostmi a customery
/// </summary>
[ApiController]
[Route("api")]
public class HouseholdController : ControllerBase
{
    /// <summary>
    /// Seznam domácností
    /// </summary>
    /// <remarks>
    /// Vraci zakladni seznam domacností pro daný Sales Arrangement bez detailu.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=B6359602-67D9-4d06-BF61-8FC85F9E082C"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement-u</param>
    /// <returns><see cref="List{T}"/> where T : <see cref="Dto.HouseholdInList"/> Seznam domacnosti pro dany Sales Arrangement</returns>
    [HttpGet("household/list/{salesArrangementId:long}")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = new [] { "Domácnost" })]
    [ProducesResponseType(typeof(List<Dto.HouseholdInList>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<Dto.HouseholdInList>> GetHouseholds([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetHouseholds.GetHouseholdsRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Detail domacnosti
    /// </summary>
    /// <remarks>
    /// Vrací detail dané domácnosti včetně detailu navázaných CustomerOnSA. Příznak druh/družka (areCustomersPartners) je počítán dynamicky při každém provolání dle logiky ve společných algoritmech.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=A2A7CE0E-0E93-459e-B883-3947C26A3DB9"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="householdId">ID domacnosti</param>
    /// <returns><see cref="GetHousehold.GetHouseholdResponse"/> Detail domacnosti</returns>
    [HttpGet("household/{householdId:long}")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = new[] { "Domácnost" })]
    [ProducesResponseType(typeof(GetHousehold.GetHouseholdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetHousehold.GetHouseholdResponse> GetHousehold([FromRoute] int householdId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetHousehold.GetHouseholdRequest(householdId), cancellationToken);

    /// <summary>
    /// Smazání domácnosti
    /// </summary>
    /// <remarks>
    /// Slouží ke smazání domácnosti včetně navázaných customerů.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=C97B19D9-A165-409d-B6FF-28029D23D517"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a><br /><br />
    /// </remarks>
    /// <param name="householdId">ID domacnosti</param>
    /// <returns>ID smazané domacnosti</returns>
    [HttpDelete("household/{householdId:int}")]
    [Consumes("application/json")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = new[] { "Domácnost" })]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<int> DeleteHousehold([FromRoute] int householdId)
        => await _mediator.Send(new DeleteHousehold.DeleteHouseholdRequest(householdId));

    /// <summary>
    /// Vytvořeni nové domacnosti
    /// </summary>
    /// <remarks>
    /// Vytvoření domácnosti i s CustomerOnSA, aby domácnost nebyla prázdná.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=72EEBC25-A403-42e9-9AFD-A48CCEBC179F"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a><br /><br />
    /// </remarks>
    [HttpPost("sales-arrangement/{salesArrangementId:int}/household")]
    [Consumes("application/json")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = new[] { "Domácnost" })]
    [ProducesResponseType(typeof(Dto.HouseholdInList), StatusCodes.Status200OK)]
    public async Task<Dto.HouseholdInList> CreateHousehold([FromRoute] int salesArrangementId, [FromBody] CreateHousehold.CreateHouseholdRequest? request)
        => await _mediator.Send(request?.InfuseId(salesArrangementId) ?? throw new NobyValidationException("Payload is empty"));

    /// <summary>
    /// Update existující domácnosti
    /// </summary>
    /// <remarks>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=C56C0725-E4F2-4b3d-9729-09D845336952"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="householdId">ID domácnosti</param>
    [HttpPut("household/{householdId:int}")]
    [Consumes("application/json")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = new[] { "Domácnost" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateHousehold([FromRoute] int householdId, [FromBody] UpdateHousehold.UpdateHouseholdRequest? request)
        => await _mediator.Send(request?.InfuseId(householdId) ?? throw new NobyValidationException("Payload is empty"));

    /// <summary>
    /// Update customeru na domácnosti
    /// </summary>
    /// <remarks>
    /// Založí, smaže nebo updatuje CustomerOnSAId1 a CustomerOnSAId2 podle objektu v requestu Customer1 a Customer2.<br /><br />
    /// Zavoláním lockedIncome = true se zapíše timestamp zamknutí příjmů. Při opakovaném zavolání API s locked income = true nedochází k přepisování timestamp.<br /><br />
    /// V případě, že se mění Customer na daném householdu, dojde k znevalidnění rozběhnutých či skončených podepisovacích proseců (nutno znovu podepsat).<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=DF47934C-D0C7-46da-B13E-C3E648389EFB"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="householdId">ID domácnosti</param>
    /// <returns>CustomerOnSAId nalinkovanych customeru</returns>
    [HttpPut("household/{householdId:int}/customers")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Domácnost" })]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [ProducesResponseType(typeof(UpdateCustomers.UpdateCustomersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<UpdateCustomers.UpdateCustomersResponse> UpdateCustomers([FromRoute] int householdId, [FromBody] UpdateCustomers.UpdateCustomersRequest? request)
        => await _mediator.Send(request?.InfuseId(householdId) ?? throw new NobyValidationException("Payload is empty"));

    private readonly IMediator _mediator;
    public HouseholdController(IMediator mediator) =>  _mediator = mediator;
}
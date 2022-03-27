using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.CustomerIncome;

[ApiController]
[Route("api/customer-on-sa")]
public class CustomerIncomeController : ControllerBase
{
    /// <summary>
    /// Smazani prijmu customera
    /// </summary>
    /// <remarks>
    /// Tento endpoint se v soucasne dobe asi pouzivat nebude.<br/>
    /// <i>DS:</i> SalesArrangementService/DeleteIncome
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID prijmu ke smazani</param>
    [HttpDelete("{customerOnSAId:int}/income/{incomeId:int}")]
    [SwaggerOperation(Tags = new[] { "UC: Prijem" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task Delete([FromRoute] int customerOnSAId, [FromRoute] int incomeId, CancellationToken cancellationToken)
        => await _mediator.Send(new DeleteIncome.DeleteIncomeRequest(customerOnSAId, incomeId), cancellationToken);

    /// <summary>
    /// Update zakladnich dat o prijmech customera
    /// </summary>
    /// <remarks>
    /// V payloadu prijima kolekci prijmu zadanych na obrazovce domacnosti. Kolekci porovna se stavem ulozenym v databazi a provede rozdilove ulozeni.<br/>
    /// - pokud je v CreateIncomeItem vyplnen IncomeId, pokusi se updatovat zakladni data daneho prijmu
    /// - pokud je v CreateIncomeItem IcnomeId=NULL, vytvori novy prijem
    /// - pokud v payloadu nenajde nektery z jiz existujicich prijmu v databazi, tyto prijmy smaze
    /// Pokud uzivatel zmeni v dropdownu typ prijmu, tento se bere jako novy prijem. Frontend tedy musi zajistit, aby se dany CreateIncomeItem odstranil z kolekce a vlozil se do nej novy s novym IncomeTypeId a IncomeId=NULL.<br/>
    /// <i>DS:</i> SalesArrangementService/CreateIncome<br/>
    /// <i>DS:</i> SalesArrangementService/DeleteIncome<br/>
    /// <i>DS:</i> SalesArrangementService/UpdateIncomeBasicData
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <returns><see cref="List{T}"/> where T : <see cref="int"/> (IncomeId)</returns>
    [HttpPost("{customerOnSAId:int}/income")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Domacnost", "UC: Prijem" })]
    [ProducesResponseType(typeof(int[]), StatusCodes.Status200OK)]
    public async Task<int[]> UpdateIncomes([FromRoute] int customerOnSAId, [FromBody] UpdateIncomes.UpdateIncomesRequest? request, CancellationToken cancellationToken)
        => await _mediator.Send(request?.InfuseId(customerOnSAId) ?? throw new CisArgumentNullException(0, "Payload is empty", nameof(request)), cancellationToken);

    /// <summary>
    /// Detail prijmu customera
    /// </summary>
    /// <remarks>
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
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<object> GetDetail([FromRoute] int customerOnSAId, [FromRoute] int incomeId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetIncome.GetIncomeRequest(customerOnSAId, incomeId), cancellationToken);

    /// <summary>
    /// Update detailu prijmu customera
    /// </summary>
    /// <remarks>
    /// Pouzit pro update detailu prijmu - tj. Level 2 obrazovka prokliknuta z detailu domacnosti.<br/>
    /// <i>DS:</i> SalesArrangementService/UpdateIncome
    /// </remarks>
    /// <param name="customerOnSAId">ID customera</param>
    /// <param name="incomeId">ID prijmu</param>
    [HttpPut("{customerOnSAId:int}/income/{incomeId:int}")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Prijem" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task Update([FromRoute] int customerOnSAId, [FromRoute] int incomeId, [FromBody] UpdateIncome.UpdateIncomeRequest? request, CancellationToken cancellationToken)
        => await _mediator.Send(request?.InfuseId(customerOnSAId, incomeId) ?? throw new CisArgumentNullException(0, "Payload is empty", nameof(request)), cancellationToken);

    private readonly IMediator _mediator;
    public CustomerIncomeController(IMediator mediator) => _mediator = mediator;
}

using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.SalesArrangement;

[ApiController]
[Route("api/sales-arrangement")]
public class SalesArrangementController : ControllerBase
{
    /// <summary>
    /// Validace dat SalesArrangementu - checkform.
    /// </summary>
    /// <remarks>
    /// Provolání SB metody Checkform pro kontrolu správnosti vyplnění SalesArrangementu.<br />
    /// Data z doménové služby jsou roztříděné do kategorií a seřazeny abecedně v rámci kategorie podle parametru 'parameter'<br /><br />
    /// <i>DS:</i> SalesArrangementService/validateSalesArrangement
    /// </remarks>
    [HttpGet("{salesArrangementId:int}/validate")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(typeof(IEnumerable<NOBY.Infrastructure.ErrorHandling.ApiErrorItem>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Validate.ValidateResponse), StatusCodes.Status200OK)]
    public async Task<Validate.ValidateResponse> Validate([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new Validate.ValidateRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Smazaní SalesArrangement-u
    /// </summary>
    /// <remarks>
    /// Smazání pouze servisních žádostí (validace na servisní žádosti je v doménových službách).<br /><br />
    /// <i>DS:</i>SalesArrangementService/DeleteSalesArrangement
    /// </remarks>
    /// <param name="salesArrangementId">ID</param>
    [HttpDelete("{salesArrangementId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<NOBY.Infrastructure.ErrorHandling.ApiErrorItem>), StatusCodes.Status400BadRequest)]
    public async Task DeleteSalesArrangement([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new DeleteSalesArrangement.DeleteSalesArrangementRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Vrací vyhodnocení dané úvěrové žádosti
    /// </summary>
    /// <remarks>
    /// Použít pro Skóring - výsledek vyhodnocení<br/>
    /// - výsledek vyhodnocení žádosti<br/>
    /// - výsledek vyhodnocení za jednotlivé domácnosti<br/>
    /// Možno vyžadovat nové vyhodnocení
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement-u</param>
    /// <param name="newAssessmentRequired">Požadováno nové posouzení</param>
    /// <returns><see cref="GetLoanApplicationAssessment.GetLoanApplicationAssessmentResponse"/> Vysledek</returns>
    [HttpGet("{salesArrangementId:int}/loan-application-assessment")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(typeof(GetLoanApplicationAssessment.GetLoanApplicationAssessmentResponse), StatusCodes.Status200OK)]
    public async Task<GetLoanApplicationAssessment.GetLoanApplicationAssessmentResponse> GetLoanApplicationAssessment([FromRoute] int salesArrangementId, [FromQuery] bool newAssessmentRequired, CancellationToken cancellationToken)
        => await _mediator.Send(new GetLoanApplicationAssessment.GetLoanApplicationAssessmentRequest(salesArrangementId, newAssessmentRequired), cancellationToken);

    /// <summary>
    /// Výpočet rozšírené bonity
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> HouseholdService<br/>
    /// <i>DS:</i> CustomerOnSaService
    /// </remarks>
    /// <param name="salesArrangementId">Sales arrangement</param>
    /// <returns><see cref="GetCreditWorthiness.GetCreditWorthinessResponse"/> Výsledek výpočtu</returns>
    [HttpGet("{salesArrangementId:int}/credit-worthiness")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(typeof(GetCreditWorthiness.GetCreditWorthinessResponse), StatusCodes.Status200OK)]
    public async Task<GetCreditWorthiness.GetCreditWorthinessResponse> GetCreditWorthiness([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCreditWorthiness.GetCreditWorthinessRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Seznam Sales Arrangements pro Case.
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/GetSalesArrangementList
    /// </remarks>
    /// <param name="caseId">ID Case-u</param>
    /// <returns><see cref="List{T}"/> where T : <see cref="Dto.SalesArrangementListItem"/> Seznam zakladních informací o všech Sales Arrangements pro daný Case.</returns>
    [HttpGet("list/{caseId:long}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "Sales Arrangement" })]
    [ProducesResponseType(typeof(List<Dto.SalesArrangementListItem>), StatusCodes.Status200OK)]
    public async Task<List<Dto.SalesArrangementListItem>> GetList([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetList.GetListRequest(caseId), cancellationToken);

    /// <summary>
    /// Seznam klientů navázaných na Sales Arrangement.
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/GetCustomerList<br/>
    /// <i>DS:</i> CustomerService/GetCustomerDetail
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    /// <returns><see cref="List{T}"/> where T : <see cref="Dto.CustomerListItem"/> Seznam klientů vč. všech jejich dat dotažených z CM atd.</returns>
    [HttpGet("{salesArrangementId:int}/customers")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "Sales Arrangement" })]
    [ProducesResponseType(typeof(List<Dto.CustomerListItem>), StatusCodes.Status200OK)]
    public async Task<List<Dto.CustomerListItem>> GetCustomers([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCustomers.GetCustomersRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Detail Sales Arrangement-u.
    /// </summary>
    /// <remarks>
    /// <i>DS:SalesArrangementService/GetDetail</i><br />
    /// Obsahuje kompilaci údajů z SA a navázené Offer. Pro každý typ produktu se vrací jiná struktura Data objektu.
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    [HttpGet("{salesArrangementId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "Sales Arrangement" })]
    [ProducesResponseType(typeof(GetDetail.GetDetailResponse), StatusCodes.Status200OK)]
    public async Task<GetDetail.GetDetailResponse> GetDetail([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetDetail.GetDetailRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Update dat SalesArrangement-u
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/UpdateSalesArrangementParameters
    /// </remarks>
    [HttpPut("{salesArrangementId:int}/parameters")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task UpdateParameters([FromRoute] int salesArrangementId, [FromBody] UpdateParameters.UpdateParametersRequest request)
        => await _mediator.Send(request.InfuseId(salesArrangementId));

    /// <summary>
    /// Validace SalesArrangementu a odeslání do StarBuildu
    /// </summary>
    /// <remarks>
    /// Dojde ke zvalidování obsahu žádosti stejně, jako při operaci validace a předání datových vět na rozhraní Starbuildu.<br /><br />
    /// Pokud žádost obsahuje chyby, které nejsou/nemohou být ignorovány, vrací se chyba.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=DF791F57-8B9E-41b2-94C1-7D73A5B30BBB"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="ignoreWarnings">Ignorovat varování a odeslat do Starbuildu</param>
    [HttpPost("{salesArrangementId:int}/send")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<NOBY.Infrastructure.ErrorHandling.ApiErrorItem>), StatusCodes.Status400BadRequest)]
    public async Task SendToCmp([FromRoute] int salesArrangementId, [FromQuery] bool ignoreWarnings = false)
        => await _mediator.Send(new SendToCmp.SendToCmpRequest(salesArrangementId, ignoreWarnings));

    private readonly IMediator _mediator;
    public SalesArrangementController(IMediator mediator) =>  _mediator = mediator;
}

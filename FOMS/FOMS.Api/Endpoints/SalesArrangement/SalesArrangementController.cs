using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.SalesArrangement;

[ApiController]
[Route("api/sales-arrangement")]
public class SalesArrangementController : ControllerBase
{
    /// <summary>
    /// Validace dat SalesArrangementu - checkform.
    /// </summary>
    /// <remarks>
    /// Provolání SB metody Checkform pro kontrolu správnosti vyplnění SalesArrangementu.<br /><br />
    /// <i>DS:</i> SalesArrangementService/validateSalesArrangement
    /// </remarks>
    [HttpGet("{salesArrangementId:int}/validate")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: SalesArrangement" })]
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
    [SwaggerOperation(Tags = new[] { "UC: SalesArrangement" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    //[SwaggerOperation(Tags = new[] { "UC: Domacnost" })]
    [ProducesResponseType(typeof(GetLoanApplicationAssessment.GetLoanApplicationAssessmentResponse), StatusCodes.Status200OK)]
    public async Task<GetLoanApplicationAssessment.GetLoanApplicationAssessmentResponse> GetLoanApplicationAssessment([FromRoute] int salesArrangementId, [FromQuery] bool newAssessmentRequired, CancellationToken cancellationToken)
        => await _mediator.Send(new GetLoanApplicationAssessment.GetLoanApplicationAssessmentRequest(salesArrangementId, newAssessmentRequired), cancellationToken);

    /// <summary>
    /// Vypocet rozsirene bonity
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> HouseholdService<br/>
    /// <i>DS:</i> CustomerOnSaService
    /// </remarks>
    /// <param name="salesArrangementId">Sales arrangement</param>
    /// <returns><see cref="GetCreditWorthiness.GetCreditWorthinessResponse"/> Vysledek vypoctu</returns>
    [HttpGet("{salesArrangementId:int}/credit-worthiness")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Domacnost" })]
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
    /// <returns><see cref="List{T}"/> where T : <see cref="Dto.SalesArrangementListItem"/> Seznam zakladnich informaci o vsech Sales Arrangements pro dany Case.</returns>
    [HttpGet("list/{caseId:long}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Case Detail" })]
    [ProducesResponseType(typeof(List<Dto.SalesArrangementListItem>), StatusCodes.Status200OK)]
    public async Task<List<Dto.SalesArrangementListItem>> GetList([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetList.GetListRequest(caseId), cancellationToken);

    /// <summary>
    /// Seznam klientu navazanych na Sales Arrangement.
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/GetCustomerList<br/>
    /// <i>DS:</i> CustomerService/GetCustomerDetail
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    /// <returns><see cref="List{T}"/> where T : <see cref="Dto.CustomerListItem"/> Seznam klientu vc. vsech jejich dat dotazenych z CM atd.</returns>
    [HttpGet("{salesArrangementId:int}/customers")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Case Detail" })]
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
    [SwaggerOperation(Tags = new [] { "UC: Case Detail", "US: SalesArrangement" })]
    [ProducesResponseType(typeof(GetDetail.GetDetailResponse), StatusCodes.Status200OK)]
    public async Task<GetDetail.GetDetailResponse> GetDetail([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetDetail.GetDetailRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Update ostatnich parametru produktu / uveru.
    /// </summary>
    [HttpPut("{salesArrangementId:int}/parameters")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Ostatni parametry" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task UpdateParameters([FromRoute] int salesArrangementId, [FromBody] UpdateParameters.UpdateParametersRequest request)
        => await _mediator.Send(request.InfuseId(salesArrangementId));

    /// <summary>
    /// Odeslani SA do SB
    /// </summary>
    [HttpPut("{salesArrangementId:int}/send")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: SendToCmp" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<SendToCmp.SendToCmpResponse> SendToCmp([FromRoute] int salesArrangementId)
        => await _mediator.Send(new SendToCmp.SendToCmpRequest{ SalesArrangementId = salesArrangementId });

    private readonly IMediator _mediator;
    public SalesArrangementController(IMediator mediator) =>  _mediator = mediator;
}

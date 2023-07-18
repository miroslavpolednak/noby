using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.SalesArrangement;

[ApiController]
[Route("api/sales-arrangement")]
public class SalesArrangementController : ControllerBase
{
    /// <summary>
    /// Detailní informace pro sluníčko
    /// </summary>
    /// <remarks>
    /// Provede vyhodnocení klapek na základě konfigurace v konfiguračním excelu a vrátí informace nutné pro správné zobrazení rozcestníku žádosti (sluníčka).<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=2709EE14-C343-4c7f-B733-A092E41EA839"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    [HttpGet("{salesArrangementId:int}/flow")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(typeof(GetFlowSwitches.GetFlowSwitchesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetFlowSwitches.GetFlowSwitchesResponse> GetFlowSwitches([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetFlowSwitches.GetFlowSwitchesRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Validace dat SalesArrangementu - checkform.
    /// </summary>
    /// <remarks>
    /// Provolání SB metody Checkform pro kontrolu správnosti vyplnění SalesArrangementu.<br />
    /// Data z doménové služby jsou roztříděné do kategorií a seřazeny abecedně v rámci kategorie podle parametru 'parameter'<br /><br />
    /// <i>DS:</i> SalesArrangementService/validateSalesArrangement
    /// </remarks>
    [HttpGet("{salesArrangementId:int}/validate")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(typeof(ValidateSalesArrangement.ValidateSalesArrangementResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ValidateSalesArrangement.ValidateSalesArrangementResponse> ValidateSalesArrangement([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new ValidateSalesArrangement.ValidateSalesArrangementRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Smazaní SalesArrangement-u
    /// </summary>
    /// <remarks>
    /// Smazání pouze servisních žádostí (validace na servisní žádosti je v doménových službách).<br /><br />
    /// <i>DS:</i>SalesArrangementService/DeleteSalesArrangement
    /// </remarks>
    /// <param name="salesArrangementId">ID</param>
    [HttpDelete("{salesArrangementId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task DeleteSalesArrangement([FromRoute] int salesArrangementId)
        => await _mediator.Send(new DeleteSalesArrangement.DeleteSalesArrangementRequest(salesArrangementId));

    /// <summary>
    /// Vrací vyhodnocení dané úvěrové žádosti
    /// </summary>
    /// <remarks>
    /// Použít pro Skóring - výsledek vyhodnocení<br /> 
    /// - výsledek vyhodnocení žádosti<br />
    /// - výsledek vyhodnocení za jednotlivé domácnosti<br />
    /// Možno vyžadovat nové vyhodnocení<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=CB8E73AD-C282-4555-B587-A571EE896E81"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement-u</param>
    /// <param name="newAssessmentRequired">Požadováno nové posouzení</param>
    /// <returns><see cref="GetLoanApplicationAssessment.GetLoanApplicationAssessmentResponse"/> Vysledek</returns>
    [HttpGet("{salesArrangementId:int}/loan-application-assessment")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(typeof(GetLoanApplicationAssessment.GetLoanApplicationAssessmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetCreditWorthiness.GetCreditWorthinessResponse> GetCreditWorthiness([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCreditWorthiness.GetCreditWorthinessRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Seznam Sales Arrangements pro Case.
    /// </summary>
    /// <remarks>
    /// Operace slouží k získání Sales Arrangements k danému ID obchodního případu (caseId). <br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=3A82DF69-AF0D-4ec4-8263-D42354C4891E"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="caseId">ID Case-u</param>
    /// <returns><see cref="List{T}"/> where T : <see cref="Dto.SalesArrangementListItem"/> Seznam zakladních informací o všech Sales Arrangements pro daný Case.</returns>
    [HttpGet("list/{caseId:long}")]
    [AuthorizeCaseOwner]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "Sales Arrangement" })]
    [ProducesResponseType(typeof(List<Dto.SalesArrangementListItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<Dto.SalesArrangementListItem>> GetSalesArrangements([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetSalesArrangements.GetSalesArrangementsRequest(caseId), cancellationToken);

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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(typeof(GetSalesArrangement.GetSalesArrangementResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetSalesArrangement.GetSalesArrangementResponse> GetSalesArrangement([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetSalesArrangement.GetSalesArrangementRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Update dat SalesArrangement-u
    /// </summary>
    /// <remarks>
    /// Aktualizuje parametry produktového či servisního SalesArrangementu. <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=18E19FC4-9238-4249-B43E-A26A9FBBC32C"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPut("{salesArrangementId:int}/parameters")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateParameters([FromRoute] int salesArrangementId, [FromBody] UpdateParameters.UpdateParametersRequest request)
        => await _mediator.Send(request.InfuseId(salesArrangementId));

    /// <summary>
    /// Validace SalesArrangementu a odeslání do StarBuildu
    /// </summary>
    /// <remarks>
    /// Dojde ke zvalidování obsahu žádosti stejně, jako při operaci validace a předání datových vět na rozhraní Starbuildu.<br /><br />
    /// Pokud žádost obsahuje chyby, které nejsou/nemohou být ignorovány, vrací se chyba.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=DF791F57-8B9E-41b2-94C1-7D73A5B30BBB"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="ignoreWarnings">Ignorovat varování a odeslat do Starbuildu</param>
    [HttpPost("{salesArrangementId:int}/send")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task SendToCmp([FromRoute] int salesArrangementId, [FromQuery] bool ignoreWarnings = false)
        => await _mediator.Send(new SendToCmp.SendToCmpRequest(salesArrangementId, ignoreWarnings));

    /// <summary>
    /// Získání komentáře na SalesArrangementu.
    /// </summary>
    /// <remarks>
    /// Vrátí komentář produktové žádosti. <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=8B0AD4B8-A056-465a-8EBB-F3E690484E4C"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("{salesArrangementId:int}/comment")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Dto.Comment> GetComment([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetComment.GetCommentRequest(salesArrangementId), cancellationToken);
    
    /// <summary>
    /// Aktualizace komentáře na SalesArrangementu.
    /// </summary>
    /// <remarks>
    /// Aktualizuje komentář produktové žádosti. <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=5792DE4C-67E9-4e3f-A47A-E4D54C79AD4B"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPut("{salesArrangementId:int}/comment")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateComment([FromRoute] int salesArrangementId, [FromBody] Dto.Comment? comment)
        => await _mediator.Send(new UpdateComment.UpdateCommentRequest(salesArrangementId, comment ?? throw new NobyValidationException("Payload is empty")));
    
    private readonly IMediator _mediator;
    public SalesArrangementController(IMediator mediator) =>  _mediator = mediator;
}

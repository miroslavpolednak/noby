﻿using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.SalesArrangement;

[ApiController]
[Route("api/sales-arrangement")]
[ApiVersion(1)]
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
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(typeof(GetFlowSwitches.GetFlowSwitchesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetFlowSwitches.GetFlowSwitchesResponse> GetFlowSwitches([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetFlowSwitches.GetFlowSwitchesRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Validace dat SalesArrangementu - checkform.
    /// </summary>
    /// <remarks>
    /// Provolání SB metody Checkform pro kontrolu správnosti vyplnění SalesArrangementu.<br /><br />
    /// Data z doménové služby jsou roztříděné do kategorií a seřazeny abecedně v rámci kategorie podle parametru 'parameter'<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=91FE5011-5C25-486e-B425-002C76448D66"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("{salesArrangementId:int}/validate")]
        [Produces("application/json")]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(typeof(ValidateSalesArrangement.ValidateSalesArrangementResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ValidateSalesArrangement.ValidateSalesArrangementResponse> ValidateSalesArrangement([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new ValidateSalesArrangement.ValidateSalesArrangementRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Smazaní SalesArrangement-u
    /// </summary>
    /// <remarks>
    ///  Smazání pouze servisních žádostí.
    ///  
    ///  <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=513586F4-297B-4e72-BF55-2207943157C6"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">ID</param>
    [HttpDelete("{salesArrangementId:int}")]
    [NobyAuthorize(UserPermissions.CHANGE_REQUESTS_Access)]
    [Produces("application/json")]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
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
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=CB8E73AD-C282-4555-B587-A571EE896E81"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement-u</param>
    /// <param name="newAssessmentRequired">Požadováno nové posouzení</param>
    /// <returns><see cref="GetLoanApplicationAssessment.V2.GetLoanApplicationAssessmentResponse"/> Vysledek</returns>
    [HttpGet("{salesArrangementId:int}/loan-application-assessment")]
    [ApiVersion("2")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(typeof(GetLoanApplicationAssessment.V2.GetLoanApplicationAssessmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetLoanApplicationAssessment.V2.GetLoanApplicationAssessmentResponse> GetLoanApplicationAssessmentV2([FromRoute] int salesArrangementId, [FromQuery] bool newAssessmentRequired, CancellationToken cancellationToken)
        => await _mediator.Send(new GetLoanApplicationAssessment.V2.GetLoanApplicationAssessmentRequest(salesArrangementId, newAssessmentRequired), cancellationToken);

    /// <summary>
    /// Výpočet rozšírené bonity
    /// </summary>
    /// <remarks>
    ///  <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=EBB933C8-328F-497d-A434-9D2D3C565CB0"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">Sales arrangement</param>
    /// <returns><see cref="GetCreditWorthiness.GetCreditWorthinessResponse"/> Výsledek výpočtu</returns>
    [HttpGet("{salesArrangementId:int}/credit-worthiness")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
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
    /// <returns><see cref="List{T}"/> where T : <see cref="SharedDto.SalesArrangementListItem"/> Seznam zakladních informací o všech Sales Arrangements pro daný Case.</returns>
    [HttpGet("list/{caseId:long}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(typeof(List<SharedDto.SalesArrangementListItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<SharedDto.SalesArrangementListItem>> GetSalesArrangements([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetSalesArrangements.GetSalesArrangementsRequest(caseId), cancellationToken);

    /// <summary>
    /// Seznam klientů navázaných na Sales Arrangement.
    /// </summary>
    /// <remarks>
    /// Vrátí seznam klientů navázaných na SalesArrangement.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=439685F4-2313-4dfe-A374-8EE45F1C8E86"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    /// <returns><see cref="List{T}"/> where T : <see cref="SharedDto.CustomerListItem"/> Seznam klientů vč. všech jejich dat dotažených z CM atd.</returns>
    [HttpGet("{salesArrangementId:int}/customers")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(typeof(List<SharedDto.CustomerListItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<SharedDto.CustomerListItem>> GetCustomersOnSa([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCustomers.GetCustomersRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Detail Sales Arrangement-u.
    /// </summary>
    /// <remarks>
    ///  Obsahuje kompilaci údajů z SA a navázené Offer. Pro každý typ produktu se vrací jiná struktura Data objektu.<br /><br />
    ///  <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=B35C47A9-4467-4949-9B03-0724D6D73F6F"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    [HttpGet("{salesArrangementId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
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
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
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
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Send, UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces("application/json")]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
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
    /// <param name="salesArrangementId">ID Sales Arrangementu</param>
    [HttpGet("{salesArrangementId:int}/comment")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<SharedDto.Comment> GetComment([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
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
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateComment([FromRoute] int salesArrangementId, [FromBody] SharedDto.Comment? comment)
        => await _mediator.Send(new UpdateComment.UpdateCommentRequest(salesArrangementId, comment ?? throw new NobyValidationException("Payload is empty")));

    private readonly IMediator _mediator;
    public SalesArrangementController(IMediator mediator) => _mediator = mediator;
}

using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.SalesArrangement;

[ApiController]
[Route("api/v{v:apiVersion}/sales-arrangement")]
[ApiVersion(1)]
public sealed class SalesArrangementController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Detailní informace pro sluníčko
    /// </summary>
    /// <remarks>
    /// Provede vyhodnocení klapek na základě konfigurace v konfiguračním excelu a vrátí informace nutné pro správné zobrazení rozcestníku žádosti (sluníčka).
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    [HttpGet("{salesArrangementId:int}/flow")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(typeof(SalesArrangementGetFlowSwitchesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=2709EE14-C343-4c7f-B733-A092E41EA839")]
    public async Task<SalesArrangementGetFlowSwitchesResponse> GetFlowSwitches([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetFlowSwitches.GetFlowSwitchesRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Validace dat SalesArrangementu - checkform.
    /// </summary>
    /// <remarks>
    /// Provolání SB metody Checkform pro kontrolu správnosti vyplnění SalesArrangementu.<br /><br />
    /// Data z doménové služby jsou roztříděné do kategorií a seřazeny abecedně v rámci kategorie podle parametru 'parameter'
    /// </remarks>
    [HttpGet("{salesArrangementId:int}/validate")]
        [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(typeof(SalesArrangementValidateSalesArrangementResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=91FE5011-5C25-486e-B425-002C76448D66")]
    public async Task<SalesArrangementValidateSalesArrangementResponse> ValidateSalesArrangement([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new ValidateSalesArrangement.ValidateSalesArrangementRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Smazaní SalesArrangement-u
    /// </summary>
    /// <remarks>
    ///  Smazání pouze servisních žádostí.
    /// </remarks>
    /// <param name="salesArrangementId">ID</param>
    [HttpDelete("{salesArrangementId:int}")]
    [NobyAuthorize(UserPermissions.CHANGE_REQUESTS_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=513586F4-297B-4e72-BF55-2207943157C6")]
    public async Task DeleteSalesArrangement([FromRoute] int salesArrangementId)
        => await _mediator.Send(new DeleteSalesArrangement.DeleteSalesArrangementRequest(salesArrangementId));

    /// <summary>
    /// Vrací vyhodnocení dané úvěrové žádosti
    /// </summary>
    /// <remarks>
    /// Použít pro Skóring - výsledek vyhodnocení<br /> 
    /// - výsledek vyhodnocení žádosti<br />
    /// - výsledek vyhodnocení za jednotlivé domácnosti<br />
    /// Možno vyžadovat nové vyhodnocení
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement-u</param>
    /// <param name="newAssessmentRequired">Požadováno nové posouzení</param>
    [HttpGet("{salesArrangementId:int}/loan-application-assessment")]
    [ApiVersion(2)]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access, UserPermissions.SCORING_Perform)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(typeof(SalesArrangementGetLoanApplicationAssessmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=CB8E73AD-C282-4555-B587-A571EE896E81")]
    public async Task<SalesArrangementGetLoanApplicationAssessmentResponse> GetLoanApplicationAssessment([FromRoute] int salesArrangementId, [FromQuery] bool newAssessmentRequired, CancellationToken cancellationToken)
        => await _mediator.Send(new GetLoanApplicationAssessment.GetLoanApplicationAssessmentRequest(salesArrangementId, newAssessmentRequired), cancellationToken);

    /// <summary>
    /// Výpočet rozšírené bonity
    /// </summary>
    /// <param name="salesArrangementId">Sales arrangement</param>
    [HttpGet("{salesArrangementId:int}/credit-worthiness")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(typeof(SalesArrangementGetCreditWorthinessResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=EBB933C8-328F-497d-A434-9D2D3C565CB0")]
    public async Task<SalesArrangementGetCreditWorthinessResponse> GetCreditWorthiness([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCreditWorthiness.GetCreditWorthinessRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Seznam Sales Arrangements pro Case.
    /// </summary>
    /// <remarks>
    /// Operace slouží k získání Sales Arrangements k danému ID obchodního případu (caseId).
    /// </remarks>
    /// <param name="caseId">ID Case-u</param>
    /// <returns><see cref="List{T}"/> where T : <see cref="SalesArrangementGetSalesArrangementsItem"/> Seznam zakladních informací o všech Sales Arrangements pro daný Case.</returns>
    [HttpGet("list/{caseId:long}")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(typeof(List<SalesArrangementGetSalesArrangementsItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=3A82DF69-AF0D-4ec4-8263-D42354C4891E")]
    public async Task<List<SalesArrangementGetSalesArrangementsItem>> GetSalesArrangements([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetSalesArrangements.GetSalesArrangementsRequest(caseId), cancellationToken);

    /// <summary>
    /// Seznam klientů navázaných na Sales Arrangement.
    /// </summary>
    /// <remarks>
    /// Vrátí seznam klientů navázaných na SalesArrangement.
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    /// <returns><see cref="List{T}"/> where T : <see cref="SalesArrangementGetCustomersOnSaItem"/> Seznam klientů vč. všech jejich dat dotažených z CM atd.</returns>
    [HttpGet("{salesArrangementId:int}/customers")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(typeof(List<SalesArrangementGetCustomersOnSaItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=439685F4-2313-4dfe-A374-8EE45F1C8E86")]
    public async Task<List<SalesArrangementGetCustomersOnSaItem>> GetCustomersOnSa([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCustomersOnSa.GetCustomersOnSaRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Detail Sales Arrangement-u.
    /// </summary>
    /// <remarks>
    ///  Obsahuje kompilaci údajů z SA a navázené Offer. Pro každý typ produktu se vrací jiná struktura Data objektu.
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    [HttpGet("{salesArrangementId:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(typeof(SalesArrangementGetSalesArrangementResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=B35C47A9-4467-4949-9B03-0724D6D73F6F")]
    public async Task<SalesArrangementGetSalesArrangementResponse> GetSalesArrangement([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetSalesArrangement.GetSalesArrangementRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Update dat SalesArrangement-u
    /// </summary>
    /// <remarks>
    /// Aktualizuje parametry produktového či servisního SalesArrangementu.
    /// </remarks>
    [HttpPut("{salesArrangementId:int}/parameters")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=18E19FC4-9238-4249-B43E-A26A9FBBC32C")]
    public async Task UpdateParameters([FromRoute] int salesArrangementId, [FromBody] SalesArrangementUpdateParametersRequest request)
        => await _mediator.Send(request.InfuseId(salesArrangementId));

    /// <summary>
    /// Validace SalesArrangementu a odeslání do StarBuildu
    /// </summary>
    /// <remarks>
    /// Dojde ke zvalidování obsahu žádosti stejně, jako při operaci validace a předání datových vět na rozhraní Starbuildu.<br /><br />
    /// Pokud žádost obsahuje chyby, které nejsou/nemohou být ignorovány, vrací se chyba.
    /// </remarks>
    /// <param name="ignoreWarnings">Ignorovat varování a odeslat do Starbuildu</param>
    [HttpPost("{salesArrangementId:int}/send")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Send, UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=DF791F57-8B9E-41b2-94C1-7D73A5B30BBB")]
    public async Task SendToCmp([FromRoute] int salesArrangementId, [FromQuery] bool ignoreWarnings = false)
        => await _mediator.Send(new SendToCmp.SendToCmpRequest(salesArrangementId, ignoreWarnings));

    /// <summary>
    /// Získání komentáře na SalesArrangementu.
    /// </summary>
    /// <remarks>
    /// Vrátí komentář produktové žádosti.
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangementu</param>
    [HttpGet("{salesArrangementId:int}/comment")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=8B0AD4B8-A056-465a-8EBB-F3E690484E4C")]
    public async Task<SalesArrangementSharedComment> GetComment([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetComment.GetCommentRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Aktualizace komentáře na SalesArrangementu.
    /// </summary>
    /// <remarks>
    /// Aktualizuje komentář produktové žádosti.
    /// </remarks>
    [HttpPut("{salesArrangementId:int}/comment")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = [ "Sales Arrangement" ])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=5792DE4C-67E9-4e3f-A47A-E4D54C79AD4B")]
    public async Task UpdateComment([FromRoute] int salesArrangementId, [FromBody] SalesArrangementUpdateCommentRequest? request)
        => await _mediator.Send(request?.InfuseId(salesArrangementId) ?? throw new NobyValidationException("Payload is empty"));
}

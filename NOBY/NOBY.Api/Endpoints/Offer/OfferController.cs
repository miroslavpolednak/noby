using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Offer;

[ApiController]
[Route("api/v{v:apiVersion}")]
[ApiVersion(1)]
public sealed class OfferController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Vezme stávající simulace k refixacím, aktuální i sdělené a přepočítá všechny s novou slevou na sazbě
    /// </summary>
    [HttpPost("case/{caseId:long}/simulate-mortgage-refixation-offer-list")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobySkipCaseStateAndProductSAValidation]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(typeof(OfferSimulateMortgageRefixationOfferListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=DAE69855-3C72-4ec4-8332-81E91814FA47")]
    public async Task<OfferSimulateMortgageRefixationOfferListResponse> SimulateMortgageRefixationOfferList([FromRoute] long caseId, [FromBody] OfferSimulateMortgageRefixationOfferListRequest request)
        => await _mediator.Send((request ?? new()).InfuseId(caseId));

    /// <summary>
    /// Simulace mimořádné splátky hypotéky.
    /// </summary>
    /// <remarks>
    /// Provolá simulační službu Starbuildu pro refixace.
    /// </remarks>
    [HttpPost("case/{caseId:long}/simulate-mortgage-extra-payment-offer")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobySkipCaseStateAndProductSAValidation]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(typeof(OfferSimulateMortgageExtraPaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=F3564317-C787-445d-AB54-A6B4FE1847C0")]
    public async Task<OfferSimulateMortgageExtraPaymentResponse> SimulateMortgageExtraPayment([FromRoute] long caseId, [FromBody] OfferSimulateMortgageExtraPaymentRequest request)
        => await _mediator.Send((request ?? new()).InfuseId(caseId));

    /// <summary>
    /// Simulace KB refixací.
    /// </summary>
    /// <remarks>
    /// Provolá simulační službu Starbuildu pro refixace.
    /// </remarks>
    [HttpPost("case/{caseId:long}/simulate-mortgage-refixation-offer")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobySkipCaseStateAndProductSAValidation]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(typeof(List<OfferSimulateMortgageRefixationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=B794F3FC-3B89-4280-AF28-449F0442ACFD")]
    public async Task<List<OfferSimulateMortgageRefixationResponse>> SimulateMortgageRefixation([FromRoute] long caseId, [FromBody] OfferSimulateMortgageRefixationRequest request)
        => await _mediator.Send((request ?? new()).InfuseId(caseId));

    /// <summary>
    /// Simulace KB retencí.
    /// </summary>
    /// <remarks>
    /// Provolá simulační službu Starbuildu pro retence.
    /// </remarks>
    [HttpPost("case/{caseId:long}/simulate-mortgage-retention-offer")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobySkipCaseStateAndProductSAValidation]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(typeof(OfferSimulateMortgageRetentionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=BFEF73A4-5876-4b5b-940B-8F65FEB5F660")]
    public async Task<OfferSimulateMortgageRetentionResponse> SimulateMortgageRetention([FromRoute] long caseId, [FromBody] OfferSimulateMortgageRetentionRequest request)
        => await _mediator.Send((request ?? new()).InfuseId(caseId));

    /// <summary>
    /// Simulace KB hypotéky.
    /// </summary>
    /// <remarks>
    /// Provolá simulační službu Starbuildu. Kromě výsledků simulace se vrací i kolekce warningů. V případě chyby simulace na straně StarBuildu se chyby zpropagují až do error response.
    /// </remarks>
    /// <param name="request">Nastaveni simulace.</param>
    /// <returns>ID vytvořené simulace a její výsledky.</returns>
    [HttpPost("offer/mortgage")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(typeof(OfferSimulateMortgageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=F8D7E9A3-7589-42af-B2B3-9B34A243D6AB")]
    public async Task<OfferSimulateMortgageResponse> SimulateMortgage([FromBody] OfferSimulateMortgageRequest request)
        => await _mediator.Send(request ?? new());

    /// <summary>
    /// Simulace stavebního spoření.
    /// </summary>
    /// <remarks>
    /// Provolá simulační službu Starbuildu. Kromě výsledků simulace se vrací i kolekce warningů.
    /// V případě chyby simulace na straně StarBuildu se chyby zpropagují až do error response.<br /><br />
    /// Formuláře simulace: <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=877723682">Confluence</a><br/><br/>
    /// Výsledek simulace: <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=876943329">Confluence</a>
    /// </remarks>
    /// <returns>ID vytvořené simulace a její výsledky.</returns>
    [HttpPost("offer/building-savings")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(typeof(OfferSimulateBuildingSavingsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=8F390B54-39D7-496d-9F1B-C40BDC60EFB2")]
    public async Task<OfferSimulateBuildingSavingsResponse> SimulateBuildingSavings([FromBody] OfferSimulateBuildingSavingsRequest request)
        => await _mediator.Send(request ?? new());

    /// <summary>
    /// Detail provedené simulace dle ID Sales Arrangement.
    /// </summary>
    /// <remarks>
    /// Stejný endpoint jako GetMortgageByOfferId, jen podle jiného ID.
    /// </remarks>
    /// <returns>Vstupy a výstupy uložené simulace.</returns>
    [HttpGet("offer/mortgage/sales-arrangement/{salesArrangementId:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(typeof(GetMortgageBySalesArrangementResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=43139152-F859-4d55-9D06-11353DA80961")]
    public async Task<GetMortgageBySalesArrangementResponse> GetMortgageBySalesArrangement([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetMortgageBySalesArrangement.GetMortgageBySalesArrangementRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Vytvoření nového případu (hypotéky) ze simulace.
    /// </summary>
    /// <remarks>
    /// Vytvoří nový Case, Sales Arrangement, Household, CustomerOnSA.<br />
    /// V případe identifikovaného klienta navíc vytvořit Product, RiskBusinessCaseId.<br />
    /// Pokud je identifikován klient, v request modelu musí být naplněna vlastnost customer.<br />
    /// Pokud se jedná o anonymní případ, musi být vyplneny vlastnosti <strong>firstName</strong> , <strong>lastName</strong> a <strong>dateOfBirth</strong>.<br /><br />
    /// Endpoint podporuje rollback.
    /// </remarks>
    /// <param name="request">Identifikace klienta a ID simulace.</param>
    /// <returns>ID nově vytvořeného Case</returns>
    [HttpPost("offer/mortgage/create-case")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.DASHBOARD_CreateNewCase)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(typeof(OfferCreateMortgageCaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=0F7EBD29-3702-4289-8AD3-9C0A44A4449C")]
    public async Task<OfferCreateMortgageCaseResponse> CreateMortgageCase([FromBody] OfferCreateMortgageCaseRequest request)
        => await _mediator.Send(request ?? new());

    /// <summary>
    /// Nastavuje příznaky na modelaci.
    /// </summary>
    [HttpPut("case/{caseId:long}/offer/{offerId:int}/flags")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task SetOfferFlags([FromRoute] long caseId, [FromRoute] int offerId, [FromBody] OfferSetOfferFlagsRequest request)
        => await _mediator.Send((request ?? new()).InfuseId(offerId));

    /// <summary>
    /// Plný splátkový kalendář dle ID simulace.
    /// </summary>
    /// <remarks>
    /// Provolá modelaci se stejnými daty jako jsou obsažena v Offer dle OfferId.Na výstupu jsou pouze data plného splátkového kalendáře a jsou potlačeny warningy.Chyba simulační služby se propaguje.
    /// </remarks>
    /// <returns>Plný splátkový kalendář simulace.</returns>
    [HttpGet("offer/mortgage/{offerId:int}/full-payment-schedule")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(typeof(OfferGetFullPaymentScheduleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=994B63B5-C1C0-4433-AB92-9FCC23760F52")]
    public async Task<OfferGetFullPaymentScheduleResponse> GetFullPaymentScheduleByOfferId([FromRoute] int offerId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetFullPaymentScheduleByOfferId.GetFullPaymentScheduleByOfferIdRequest(offerId), cancellationToken);

    /// <summary>
    /// Vyhledání developerských projektů a developerů bez projektu.
    /// </summary>
    /// <remarks>
    /// Vyhledá developerské projekty na základě vyhledávacího textu.<br /><br />
    /// Vyhledává se v číselníku <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438046695">Developer (CIS_DEVELOPER)</a> v atributech Name (NAZEV) a Cin (ICO_RC) a v číselníku 
    /// <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438046776">DeveloperProject (CIS_DEVELOPER_PROJEKTY_SPV)</a> v atributu Name (PROJEKT).<br /><br />
    /// Text se vyhledává jako subřetězce v uvedených sloupcích - ty jsou oddělené ve vyhledávacím textu mezerou.
    /// </remarks>
    [HttpPost("offer/mortgage/developer-project/search")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(typeof(OfferDeveloperSearchResponse), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=D43515EA-4014-47a1-AD45-0E80EE43AEB9")]
    public async Task<OfferDeveloperSearchResponse> DeveloperSearch([FromBody] OfferDeveloperSearchRequest request)
        => await _mediator.Send(request ?? new());

    /// <summary>
    /// Nalinkuje novou modelaci na stávající SA.
    /// </summary>
    /// <remarks>
    /// Nalinkuje novou modelaci na stávající SalesArrangement a uloží kontaktní informace pro nabídku. Pokud není identifikován hlavní dlužník, dojde k aktualizaci jména, příjmení a data narození. Pro identifikovaného dlužníka se data ignorují.
    /// </remarks>
    [HttpPut("case/{caseId:long}/sales-arrangement/{salesArrangementId:int}/link-mortgage-offer")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(typeof(OfferRefinancingLinkResult), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=8996A9D6-2732-4011-9152-0EAE7FEECE07")]
    public async Task<OfferRefinancingLinkResult> LinkMortgageOffer([FromRoute] long caseId, [FromRoute] int salesArrangementId, [FromBody][Required] OfferLinkMortgageOfferRequest request)
        => await _mediator.Send(request.InfuseId(caseId, salesArrangementId));

    /// <summary>
    /// Nalinkuje retenční nabídku na stávající SA.
    /// </summary>
    /// <remarks>
    /// Nalinkuje retenční nabídku na stávající SalesArrangement a upraví, nebo vytvoří IC workflow proces.
    /// </remarks>
    [HttpPut("case/{caseId:long}/link-mortgage-retention-offer")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_RefinancingAccess)]
    [SwaggerOperation(Tags = [ "Modelace" ])]
    [ProducesResponseType(typeof(OfferRefinancingLinkResult), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=92B4B98B-3F57-4541-9828-EB8CFDFA9035")]
    public async Task<OfferRefinancingLinkResult> LinkMortgageRetentionOffer([FromRoute] long caseId, [FromBody][Required] OfferLinkMortgageRetentionOfferRequest request)
        => await _mediator.Send(request.InfuseId(caseId));

    /// <summary>
    /// Nalinkuje mimořádnou splátku na stávající SA.
    /// </summary>
    /// <remarks>
    /// Nalinkuje mimořádnou splátku na stávající SalesArrangement a upraví, nebo vytvoří IC workflow proces.
    /// </remarks>
    [HttpPut("case/{caseId:long}/link-mortgage-extra-payment")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_RefinancingAccess)]
    [SwaggerOperation(Tags = [ "Modelace" ])]
    [ProducesResponseType(typeof(OfferRefinancingLinkResult), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=138F178A-72B5-46f6-85B2-D8414F5043B3")]
    public async Task<OfferRefinancingLinkResult> LinkMortgageExtraPayment([FromRoute] long caseId,  [FromBody][Required] OfferLinkMortgageExtraPaymentRequest request)
        => await _mediator.Send(request.InfuseId(caseId));
}
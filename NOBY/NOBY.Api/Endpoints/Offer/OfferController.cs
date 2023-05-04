using NOBY.Api.Endpoints.Offer.DeveloperSearch;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Offer;

[ApiController]
[Route("api/offer")]
public class OfferController : ControllerBase
{
    private readonly IMediator _mediator;
    public OfferController(IMediator mediator) =>  _mediator = mediator;

    /// <summary>
    /// Simulace KB hypotéky.
    /// </summary>
    /// <remarks>
    /// Provolá simulační službu Starbuildu. Kromě výsledků simulace se vrací i kolekce warningů. V případě chyby simulace na straně StarBuildu se chyby zpropagují až do error response.
    /// </remarks>
    /// <param name="request">Nastaveni simulace.</param>
    /// <returns>ID vytvořené simulace a její výsledky.</returns>
    [HttpPost("mortgage")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new [] { "Modelace" })]
    [ProducesResponseType(typeof(SimulateMortgage.SimulateMortgageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<SimulateMortgage.SimulateMortgageResponse> SimulateMortgage([FromBody] SimulateMortgage.SimulateMortgageRequest request)
        => await _mediator.Send(request);
    
    /// <summary>
    /// Detail provedené simulace dle ID simulace.
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> OfferService/GetMortgageData
    /// </remarks>
    /// <returns>Vstupy a výstupy uložené simulace.</returns>
    [HttpGet("mortgage/{offerId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "Modelace" })]
    [ProducesResponseType(typeof(Dto.GetMortgageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Dto.GetMortgageResponse> GetMortgageByOfferId([FromRoute] int offerId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetMortgageByOfferId.GetMortgageByOfferIdRequest(offerId), cancellationToken);
    
    /// <summary>
    /// Detail provedené simulace dle ID Sales Arrangement.
    /// </summary>
    /// <remarks>
    /// Stejný endpoint jako GetMortgageByOfferId, jen podle jiného ID.<br/>
    /// <i>DS:</i> SalesArrangementService/GetSalesArrangement (to get OfferId)<br/>
    /// <i>DS:</i> OfferService/GetMortgageData
    /// </remarks>
    /// <returns>Vstupy a výstupy uložené simulace.</returns>
    [HttpGet("mortgage/sales-arrangement/{salesArrangementId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "Modelace" })]
    [ProducesResponseType(typeof(Dto.GetMortgageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Dto.GetMortgageResponse> GetMortgageBySalesArrangement([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetMortgageBySalesArrangement.GetMortgageBySalesArrangementRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Vytvoření nového případu (hypotéky) ze simulace.
    /// </summary>
    /// <remarks>
    /// Vytvoří nový Case, Sales Arrangement, Household, CustomerOnSA.<br/>
    /// V případe identifikovaného klienta navíc vytvořit Product, RiskBusinessCaseId.<br/>
    /// Pokud je identifikován klient, v request modelu musí být naplněna vlastnost customer.<br/>
    /// Pokud se jedná o anonymní případ, musi být vyplneny vlastnosti <strong>firstName</strong> , <strong>lastName</strong> a <strong>dateOfBirth</strong>.<br/><br/>
    /// <i>DS:</i> OfferService/GetMortgageData<br/>
    /// <i>DS:</i> SalesArrangement/GetSalesArrangementByOfferId<br/>
    /// <i>DS:</i> CaseService/CreateCase<br/>
    /// <i>DS:</i> SalesArrangement/CreateSalesArrangement<br/>
    /// <i>DS:</i> ProductService/CreateMortgage (pokud MpHome API na vytvoření produktu spadne na fatální chybu, tak tuto chybu ignorujeme a pokračujeme ve vytvoření Case)<br/>
    /// <i>DS:</i> SalesArrangement/CreateHousehold<br/>
    /// <i>DS:</i> SalesArrangement/CreateCustomerOnSA
    /// </remarks>
    /// <param name="request">Identifikace klienta a ID simulace.</param>
    /// <returns>ID nově vytvořeného Case</returns>
    [HttpPost("mortgage/create-case")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new [] { "Modelace" })]
    [ProducesResponseType(typeof(CreateMortgageCase.CreateMortgageCaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<CreateMortgageCase.CreateMortgageCaseResponse> CreateMortgageCase([FromBody] CreateMortgageCase.CreateMortgageCaseRequest request)
        => await _mediator.Send(request);

    /// <summary>
    /// Nalinkuje novou modelaci na stávající SA.
    /// </summary>
    /// <remarks>
    /// Nalinkuje novou modelaci na stávající SalesArrangement a uloží kontaktní informace pro nabídku. Pokud není identifikován hlavní dlužník, dojde k aktualizaci jména, příjmení a data narození. Pro identifikovaného dlužníka se data ignorují.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=8996A9D6-2732-4011-9152-0EAE7FEECE07"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPut("mortgage/sales-arrangement/link")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Modelace" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task LinkModelation([FromBody] LinkModelation.LinkModelationRequest request)
        => await _mediator.Send(request);

    /// <summary>
    /// Plný splátkový kalendář dle ID simulace.
    /// </summary>
    /// <remarks>
    /// Provolá modelaci se stejnými daty jako jsou obsažena v Offer dle OfferId. Na výstupu jsou pouze data plného splátkového kalendáře a jsou potlačeny warningy. Chyba simulační služby se propaguje.<br/>
    /// <i>DS:</i> OfferService/GetMortgageOfferFPSchedule
    /// </remarks>
    /// <returns>Plný splátkový kalendář simulace.</returns>
    [HttpGet("mortgage/{offerId:int}/full-payment-schedule")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Modelace" })]
    [ProducesResponseType(typeof(Dto.GetFullPaymentScheduleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Dto.GetFullPaymentScheduleResponse> GetFullPaymentScheduleByOfferId([FromRoute] int offerId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetFullPaymentScheduleByOfferId.GetFullPaymentScheduleByOfferIdRequest(offerId), cancellationToken);
    
    /// <summary>
    /// Vyhledání developerských projektů a developerů bez projektu.
    /// </summary>
    /// <remarks>
    /// Vyhledá developerské projekty na základě vyhledávacího textu.<br />
    /// Vyhledává se v číselníku <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438046695">Developer (CIS_DEVELOPER)</a> v atributech Name (NAZEV) a Cin (ICO_RC) a v číselníku <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438046776">DeveloperProject (CIS_DEVELOPER_PROJEKTY_SPV)</a> v atributu Name (PROJEKT).<br />
    /// Text se vyhledává jako subřetězce v uvedených sloupcích - ty jsou oddělené ve vyhledávacím textu mezerou.
    /// </remarks>
    [HttpPost("mortgage/developer-project/search")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Modelace" })]
    [ProducesResponseType(typeof(DeveloperSearchResponse), StatusCodes.Status200OK)]
    public async Task<DeveloperSearchResponse> DeveloperSearch([FromBody] DeveloperSearchRequest request)
        => await _mediator.Send(request);
}
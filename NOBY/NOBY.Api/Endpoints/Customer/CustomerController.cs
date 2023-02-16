using NOBY.Infrastructure.ErrorHandling;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Customer;

[ApiController]
[Route("api")]
public class CustomerController : ControllerBase
{
    /// <summary>
    /// Založení nového klienta
    /// </summary>
    /// <remarks>
    /// Vytvoření Customer entity, která zaštiťuje KB customera a MPSS customera.<br /><br />
    /// SalesArrnagementService/UpdateCustomer(onSA)<br />CustomerService/GetDetail<br /><br />
    /// Na výstupu je Customer objekt s kompletními daty z KB CM.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=3DF2C802-9657-4400-9E31-E3B0D3E36E2D"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("customer")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient" })]
    [ProducesResponseType(typeof(Create.CreateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<NOBY.Infrastructure.ErrorHandling.ApiErrorItem>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<Create.CreateResponse> Create([FromBody] Create.CreateRequest resquest, CancellationToken cancellationToken)
        => await _mediator.Send(resquest, cancellationToken);

    /// <summary>
    /// Vyhledávání klientů
    /// </summary>
    /// <remarks>
    /// Endpoint umožňuje:
    /// - hledat podle zadaných kriterií
    /// - nastavit stránkovaní
    /// - nastavit řazení [lastName]
    /// <i>DS:</i> CustomerService/SearchCustomers
    /// </remarks>
    /// <returns>Seznam nalezených klientů. BE služba není stránkovatelná, takže stránkovaní je jen jako fake na FE.</returns>
    [HttpPost("customer/search")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new [] { "Klient" })]
    [ProducesResponseType(typeof(Search.SearchResponse), StatusCodes.Status200OK)]
    public async Task<Search.SearchResponse> Search([FromBody] Search.SearchRequest resquest, CancellationToken cancellationToken)
        => await _mediator.Send(resquest, cancellationToken);
    
    /// <summary>
    /// Detail klienta
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> CustomerService/GetCustomer
    /// </remarks>
    /// <returns>Kompletní detail klienta vrácený z KB CM nebo KonsDb.</returns>
    [HttpPost("customer/get")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "Klient" })]
    [ProducesResponseType(typeof(GetDetail.GetDetailResponse), StatusCodes.Status200OK)]
    public async Task<GetDetail.GetDetailResponse> GetDetail([FromBody] CIS.Foms.Types.CustomerIdentity request, CancellationToken cancellationToken)
        => await _mediator.Send(new GetDetail.GetDetailRequest(request.Id, request.Scheme), cancellationToken);

    /// <summary>
    /// Identifikace klienta
    /// </summary>
    /// <remarks>
    /// Slouží pro identifikaci klienta.<br />
    /// Možné použití pro hlavního dlužníka i pro spoludlužníka, na Domácnosti, na Modelaci hypotéky.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=EF40D23F-A77A-4a04-AA79-38779970393E"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("customer-on-sa/{customerOnSAId:int}/identify-by-identity")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<ApiErrorItem>), StatusCodes.Status404NotFound)]
    public async Task IdentifyByIdentity([FromRoute] int customerOnSAId, [FromBody] IdentifyByIdentity.IdentifyByIdentityRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request.InfuseId(customerOnSAId), cancellationToken);

    /// <summary>
    /// Identifikace klienta
    /// </summary>
    /// <remarks>
    /// Identikace customera probíhá provoláním <i>DS:</i> CustomerService/SearchCustomers a vrátí <b>právě jednoho nalezeného klienta</b>, případně null / HTTP 204 při nenalezení klienta v KB customer managementu.<br /><br />
    /// V případě shody s více klienty KB customer managementu dojde k vrácení chyby a zalogování duplicitních KBID.
    /// </remarks>
    [HttpPost("customer/identify")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient" })]
    [ProducesResponseType(typeof(Search.Dto.CustomerInList), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<Search.Dto.CustomerInList> Identify([FromBody] Identify.IdentifyRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);

    /// <summary>
    /// Profile check s profilem identifikovaný
    /// </summary>
    /// <remarks>
    /// Provolá <i>DS: CustomerService/profileCheck</i> s profilem 'Subjekt s identifikací' a vrátí informaci, zda profil splňuje.
    /// </remarks>
    [HttpPost("customer/profile-check")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient" })]
    [ProducesResponseType(typeof(ProfileCheck.ProfileCheckResponse), StatusCodes.Status200OK)]
    public async Task<ProfileCheck.ProfileCheckResponse> ProfileCheck([FromBody] CIS.Foms.Types.CustomerIdentity request, CancellationToken cancellationToken)
        => await _mediator.Send(new ProfileCheck.ProfileCheckRequest(request.Id, request.Scheme), cancellationToken);

    /// <summary>
    /// Získání dat klienta (s lokálními změnami)
    /// </summary>
    /// <remarks>
    /// Vrací data klienta aktualizovaného o lokálně uložené změny. Podle kontextu produktu vracíme data z KB CM (pro červené produkty) nebo z KonsDB (pro modré produkty, prozatím nepodporováno).<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=6452CB93-41C7-450f-A20F-E8CB5208F1DE"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("customer-on-sa/{customerOnSAId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient" })]
    [ProducesResponseType(typeof(IEnumerable<NOBY.Infrastructure.ErrorHandling.ApiErrorItem>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(GetDetailWithChanges.GetDetailWithChangesResponse), StatusCodes.Status200OK)]
    public async Task<GetDetailWithChanges.GetDetailWithChangesResponse> GetDetailWithChanges([FromRoute] int customerOnSAId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetDetailWithChanges.GetDetailWithChangesRequest(customerOnSAId), cancellationToken);

    /// <summary>
    /// Uložení dat klienta (pro uložení změn lokálně)
    /// </summary>
    /// <remarks>
    /// Ukládá data klienta (deltu oproti KB CM pro červené produkty, do budoucna i deltu oproti KonsDB pro modré produkty). <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=BB5766C4-CCC7-487e-B482-1B1C86D999F7"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPut("customer-on-sa/{customerOnSAId:int}")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Klient" })]
    [ProducesResponseType(typeof(IEnumerable<ApiErrorItem>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task GetDetailWithChanges([FromRoute] int customerOnSAId, [FromBody] UpdateDetailWithChanges.UpdateDetailWithChangesRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request.InfuseId(customerOnSAId), cancellationToken);

    private readonly IMediator _mediator;
    public CustomerController(IMediator mediator) =>  _mediator = mediator;
}
using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Customer;

[ApiController]
[Route("api/v{v:apiVersion}")]
[ApiVersion(1)]
public class CustomerController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Založení nového klienta
    /// </summary>
    /// <remarks>
    /// Vytvoření Customer entity, která zaštiťuje KB customera a MPSS customera.<br /><br />
    /// Na výstupu je Customer objekt s kompletními daty z KB CM.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=3DF2C802-9657-4400-9E31-E3B0D3E36E2D"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("customer-on-sa/{customerOnSAId:int}/identity")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.CLIENT_Modify)]
    [SwaggerOperation(Tags = ["Klient"])]
    [ProducesResponseType(typeof(CreateCustomer.CreateCustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<CreateCustomer.CreateCustomerResponse> CreateCustomer([FromRoute] int customerOnSAId, [FromBody] CreateCustomer.CreateCustomerRequest request)
        => await _mediator.Send(request.InfuseId(customerOnSAId));

    /// <summary>
    /// Vyhledávání klientů
    /// </summary>
    /// <remarks>
    /// Endpoint umožňuje:<br />
    /// - hledat podle zadaných kriterií<br />
    /// - nastavit stránkovaní<br />
    /// - nastavit řazení [lastName]<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=F2D0EA12-6E96-4eea-92A6-D179E99B0E2B"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <returns>Seznam nalezených klientů. BE služba není stránkovatelná, takže stránkovaní je jen jako fake na FE.</returns>
    [HttpPost("customer/search")]
    [NobyAuthorize(UserPermissions.CLIENT_SearchPerson)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient"])]
    [ProducesResponseType(typeof(SearchCustomers.SearchCustomersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<SearchCustomers.SearchCustomersResponse?> SearchCustomers([FromBody] SearchCustomers.SearchCustomersRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);

    /// <summary>
    /// Detail klienta
    /// </summary>
    /// <remarks>
    /// Vrátí detail klienta z KB CM / KonsDB.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=7DE65D0D-8FAF-4c01-A6E1-04F69E90A753"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <returns>Kompletní detail klienta vrácený z KB CM nebo KonsDb.</returns>
    [HttpPost("customer/get")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Klient"])]
    [ProducesResponseType(typeof(GetCustomerDetail.GetCustomerDetailResponse), StatusCodes.Status200OK)]
    public async Task<GetCustomerDetail.GetCustomerDetailResponse> GetCustomerDetail([FromBody] SharedTypes.Types.CustomerIdentity request) 
        => await _mediator.Send(new GetCustomerDetail.GetCustomerDetailRequest(request.Id, request.Scheme));

    /// <summary>
    /// Identifikace klienta
    /// </summary>
    /// <remarks>
    /// Slouží pro identifikaci klienta.<br />
    /// Možné použití pro hlavního dlužníka i pro spoludlužníka, na Domácnosti, na Modelaci hypotéky.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=EF40D23F-A77A-4a04-AA79-38779970393E"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("customer-on-sa/{customerOnSAId:int}/identify-by-identity")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.CLIENT_Modify)]
    [SwaggerOperation(Tags = ["Klient"])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task IdentifyByIdentity([FromRoute] int customerOnSAId, [FromBody] IdentifyByIdentity.IdentifyByIdentityRequest request)
        => await _mediator.Send(request.InfuseId(customerOnSAId));

    /// <summary>
    /// Identifikace klienta
    /// </summary>
    /// <remarks>
    /// Identikace customera probíhá provoláním <i>DS:</i> CustomerService/SearchCustomers a vrátí <b>právě jednoho nalezeného klienta</b>, případně null / HTTP 204 při nenalezení klienta v KB customer managementu.<br /><br /><br />
    /// V případě shody s více klienty KB customer managementu dojde k vrácení chyby a zalogování duplicitních KBID.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=E62ABB4D-8480-4b3e-BD13-1A1B83F5C740"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("customer/identify")]
    [NobyAuthorize(UserPermissions.CLIENT_IdentifyPerson)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient"])]
    [ProducesResponseType(typeof(SearchCustomers.Dto.CustomerInList), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<SearchCustomers.Dto.CustomerInList?> Identify([FromBody] Identify.IdentifyRequest request)
        => await _mediator.Send(request);

    /// <summary>
    /// Validace emailu či telefonního čísla
    /// </summary>
    /// <remarks>
    /// Validace kontatů (email či telefon) pomocí <i>DS:</i> CustomerService/validateContacts. Pro telefonní čísla vrací i informaci, zda se jedná o mobilní číslo.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=C74DFCBB-3F27-4bd1-A9D7-5DCE923AC862"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("contact/validate")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient"])]
    [ProducesResponseType(typeof(ValidateContact.ValidateContactResponse), StatusCodes.Status200OK)]
    public async Task<ValidateContact.ValidateContactResponse> ValidateContact([FromBody] ValidateContact.ValidateContactRequest request)
        => await _mediator.Send(request);

    /// <summary>
    /// Profile check s profilem identifikovaný
    /// </summary>
    /// <remarks>
    /// Provolá <i>DS: CustomerService/profileCheck</i> s profilem 'Subjekt s identifikací' a vrátí informaci, zda profil splňuje.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=F2992BC4-A1DB-4e57-B037-5F99244CC1D4"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("customer/profile-check")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient"])]
    [ProducesResponseType(typeof(ProfileCheck.ProfileCheckResponse), StatusCodes.Status200OK)]
    public async Task<ProfileCheck.ProfileCheckResponse> ProfileCheck([FromBody] SharedTypes.Types.CustomerIdentity request)
        => await _mediator.Send(new ProfileCheck.ProfileCheckRequest(request.Id, request.Scheme));

    /// <summary>
    /// Získání dat klienta (s lokálními změnami)
    /// </summary>
    /// <remarks>
    /// Vrací data klienta aktualizovaného o lokálně uložené změny. Podle kontextu produktu vracíme data z KB CM (pro červené produkty) nebo z KonsDB (pro modré produkty, prozatím nepodporováno).<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=6452CB93-41C7-450f-A20F-E8CB5208F1DE"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("customer-on-sa/{customerOnSAId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient"])]
    [ProducesResponseType(typeof(GetCustomerDetailWithChanges.GetCustomerDetailWithChangesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetCustomerDetailWithChanges.GetCustomerDetailWithChangesResponse> GetCustomerDetailWithChanges([FromRoute] int customerOnSAId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCustomerDetailWithChanges.GetCustomerDetailWithChangesRequest(customerOnSAId), cancellationToken);

    /// <summary>
    /// Uložení dat klienta (pro uložení změn lokálně)
    /// </summary>
    /// <remarks>
    /// Ukládá data klienta (deltu oproti KB CM pro červené produkty, do budoucna i deltu oproti KonsDB pro modré produkty). <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=BB5766C4-CCC7-487e-B482-1B1C86D999F7"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPut("customer-on-sa/{customerOnSAId:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.CLIENT_Modify)]
    [SwaggerOperation(Tags = ["Klient"])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateCustomerDetailWithChanges([FromRoute] int customerOnSAId, [FromBody] UpdateCustomerDetailWithChanges.UpdateCustomerDetailWithChangesRequest request)
        => await _mediator.Send(request.InfuseId(customerOnSAId));
}
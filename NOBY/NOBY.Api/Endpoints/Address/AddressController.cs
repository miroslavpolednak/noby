using System.Net.Mime;
using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Address;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}")]
public class AddressController : ControllerBase
{
    private readonly IMediator _mediator;
    public AddressController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Našeptávání adresy z AddressWhispererBEService
    /// </summary>
    /// <remarks>
    /// Provolá AddressWhispererBEService na IIB a vrátí stránkovaný seznam adres. Dotazujeme se zprostředkovaně do RUIAN pro CZ/SK adresy a přes službu Google pro ostatní.<br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=046F9631-86E1-4f17-9527-C518700030CE"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20"/>Diagram v EA</a>
    /// </remarks>
    [HttpPost("address/search")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = new[] { "Klient" })]
    [ProducesResponseType(typeof(AddressSearch.AddressSearchResponse), StatusCodes.Status200OK)]
    public async Task<AddressSearch.AddressSearchResponse> AddressSearch([FromBody] AddressSearch.AddressSearchRequest request)
        => await _mediator.Send(request);

    /// <summary>
    /// Získání detailu adresy přes AddressWhispererBEService
    /// </summary>
    /// <remarks>
    /// Provolánám získáme přes AddressWhispererBEService na IIB detail konkrétní adresy.<br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=312CA931-1DA0-4be2-AF0D-4D71962845F2"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20"/>Diagram v EA</a>
    /// </remarks>
    [HttpPost("address/detail")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = new[] { "Klient" })]
    [ProducesResponseType(typeof(GetAddressDetail.GetAddressDetailResponse), StatusCodes.Status200OK)]
    public async Task<GetAddressDetail.GetAddressDetailResponse> GetAddressDetail([FromBody] GetAddressDetail.GetAddressDetailRequest request)
        => await _mediator.Send(request);

    /// <summary>
    /// Našeptávání katastrálních území (KATUZ)
    /// </summary>
    /// <remarks>
    /// Provolá  CREM službu RUIAN:FindTerritory a vrátí stránkovaný seznam katuz setřízený abecedně podle jména katastrálního území.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=46994CE5-FC35-4ad8-97D6-5217CFBE2F27"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <returns></returns>
    [HttpPost("katuz/search")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType<KatuzSearch.KatuzSearchResponse>(StatusCodes.Status200OK)]
    public async Task<KatuzSearch.KatuzSearchResponse> KatuzSearch([FromBody] KatuzSearch.KatuzSearchRequest request, CancellationToken cancellationToken) => 
        await _mediator.Send(request, cancellationToken);
}

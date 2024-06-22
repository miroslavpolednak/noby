using Asp.Versioning;
using NOBY.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Address;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}")]
public class AddressController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Našeptávání adresy z AddressWhispererBEService
    /// </summary>
    /// <remarks>
    /// Provolá AddressWhispererBEService na IIB a vrátí stránkovaný seznam adres. Dotazujeme se zprostředkovaně do RUIAN pro CZ/SK adresy a přes službu Google pro ostatní.
    /// </remarks>
    [HttpPost("address/search")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient"])]
    [ProducesResponseType(typeof(AddressAddressSearchResponse), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=046F9631-86E1-4f17-9527-C518700030CE")]
    public async Task<AddressAddressSearchResponse> AddressSearch([FromBody] ApiContracts.AddressAddressSearchRequest request)
        => await _mediator.Send(request);

    /// <summary>
    /// Získání detailu adresy přes AddressWhispererBEService
    /// </summary>
    /// <remarks>
    /// Provolánám získáme přes AddressWhispererBEService na IIB detail konkrétní adresy.
    /// </remarks>
    [HttpPost("address/detail")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient"])]
    [ProducesResponseType(typeof(AddressGetAddressDetailResponse), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=312CA931-1DA0-4be2-AF0D-4D71962845F2")]
    public async Task<AddressGetAddressDetailResponse> GetAddressDetail([FromBody] AddressGetAddressDetailRequest request)
        => await _mediator.Send(request);

    /// <summary>
    /// Našeptávání katastrálních území (KATUZ)
    /// </summary>
    /// <remarks>
    /// Provolá  CREM službu RUIAN:FindTerritory a vrátí stránkovaný seznam katuz setřízený abecedně podle jména katastrálního území.
    /// </remarks>
    [HttpPost("katuz/search")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType<AddressKatuzSearchResponse>(StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=46994CE5-FC35-4ad8-97D6-5217CFBE2F27")]
    public async Task<AddressKatuzSearchResponse> KatuzSearch([FromBody] AddressKatuzSearchRequest request, CancellationToken cancellationToken) => 
        await _mediator.Send(request, cancellationToken);
}

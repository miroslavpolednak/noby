using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.Address;

[ApiController]
[Route("api/address")]
public class AddressController : ControllerBase
{
    private readonly IMediator _mediator;
    public AddressController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Našeptávání adresy z AddressWhispererBEService
    /// </summary>
    /// <remarks>
    /// Provolá AddressWhispererBEService na IIB a vrátí stránkovaný seznam adres. Dotazujeme se zprostředkovaně do RUIAN pro CZ/SK adresy a přes službu Google pro ostatní.<br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&o=046F9631-86E1-4f17-9527-C518700030CE"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20"/>Diagram v EA</a>
    /// </remarks>
    [HttpPost("search")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Klient" })]
    [ProducesResponseType(typeof(AddressSearch.AddressSearchResponse), StatusCodes.Status200OK)]
    public async Task<AddressSearch.AddressSearchResponse> AddressSearch([FromBody] AddressSearch.AddressSearchRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);

    /// <summary>
    /// Získání detailu adresy přes AddressWhispererBEService
    /// </summary>
    /// <remarks>
    /// Provolánám získáme přes AddressWhispererBEService na IIB detail konkrétní adresy.<br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&o=312CA931-1DA0-4be2-AF0D-4D71962845F2"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20"/>Diagram v EA</a>
    /// </remarks>
    [HttpPost("detail")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Klient" })]
    [ProducesResponseType(typeof(GetAddressDetail.GetAddressDetailResponse), StatusCodes.Status200OK)]
    public async Task<GetAddressDetail.GetAddressDetailResponse> GetAddressDetail([FromBody] GetAddressDetail.GetAddressDetailRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}

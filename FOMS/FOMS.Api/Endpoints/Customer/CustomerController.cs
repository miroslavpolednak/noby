using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.Customer;

[ApiController]
[Route("api/customer")]
public class CustomerController : ControllerBase
{
    /// <summary>
    /// [DEV] Vyhledavani klientu
    /// </summary>
    /// <remarks>
    /// DS: CustomerService/SearchCustomers
    /// </remarks>
    /// <returns>Seznam nalezenych klientu. BE sluzba neni strankovatelna, takze strankovani je jen jako fake na FE.</returns>
    [HttpPost("search")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Identifikace klienta", "UC: Domacnost" })]
    [ProducesResponseType(typeof(Search.SearchResponse), StatusCodes.Status200OK)]
    public async Task<Search.SearchResponse> Search([FromBody] Search.SearchRequest resquest, CancellationToken cancellationToken)
        => await _mediator.Send(resquest, cancellationToken);
    
    /// <summary>
    /// [DEV] Detail klienta
    /// </summary>
    /// <remarks>
    /// DS: CustomerService/GetCustomer
    /// </remarks>
    /// <param name="identityId">ID klienta v danem schematu</param>
    /// <param name="identitySchema">Schema ve kterem je klient ulozeny - Kb | Mp</param>
    /// <returns>Kompletni detail klienta vraceny z KB CM nebo KonsDb.</returns>
    [HttpGet("get")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Identifikace klienta", "UC: Domacnost" })]
    [ProducesResponseType(typeof(GetDetail.GetDetailResponse), StatusCodes.Status200OK)]
    public async Task<GetDetail.GetDetailResponse> GetDetail([FromQuery] int identityId, [FromQuery] CIS.Foms.Enums.IdentitySchemes identitySchema, CancellationToken cancellationToken)
        => await _mediator.Send(new GetDetail.GetDetailRequest(identityId, identitySchema), cancellationToken);
    
    private readonly IMediator _mediator;
    public CustomerController(IMediator mediator) =>  _mediator = mediator;
}
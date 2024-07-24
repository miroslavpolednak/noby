using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Product;

[ApiController]
[Route("api/v{v:apiVersion}/product")]
[ApiVersion(1)]
public sealed class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Vrátí seznam dlužníků a spoludlužníků na daném produktu
    /// </summary>
    /// <remarks>
    /// Vrátí seznam dlužníků a spoludlužníků na daném produktu.
    /// </remarks>
    /// <returns>Seznam klientů na produktu</returns>
    [HttpGet("{caseId:long}/customers")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Produkt"])]
    [ProducesResponseType(typeof(List<ProductGetCustomersOnProductItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=DFD7FC8B-6D32-4a81-9608-A502E6F0E74B")]
    public async Task<List<ProductGetCustomersOnProductItem>> GetCustomersOnProduct([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCustomersOnProduct.GetCustomersOnProductRequest(caseId), cancellationToken);

    /// <summary>
    /// Vrátí seznam závazků na daném produktu
    /// </summary>
    /// <remarks>
    ///Seznam z závazků načtený z KonsDB.
    /// </remarks>
    /// <returns>Seznam závazků na produktu</returns>
    [HttpGet("{caseId:long}/obligations")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Produkt"])]
    [ProducesResponseType(typeof(List<ProductGetProductObligationListObligation>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=058D1B76-375C-4747-90DB-D4E3BE5C46F7")]
    public async Task<List<ProductGetProductObligationListObligation>> GetProductObligations([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetProductObligations.GetProductObligationsRequest(caseId), cancellationToken);

    /// <summary>
    /// Vrátí Case ID podle zadaného PCP ID
    /// </summary>
    /// <remarks>
    /// Vyhledá Case ID podle PCP ID v KonsDB.
    /// </remarks>
    /// <param name="pcpId">PCP ID</param>
    [HttpGet("caseid-by-pcpid/{pcpId:required}")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Produkt"])]
    [ProducesResponseType(typeof(ProductGetCaseIdByPcpIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=048F18B9-903F-4e3a-8B79-02592E9ED41B")]
    public async Task<ProductGetCaseIdByPcpIdResponse> GetCaseIdByPcpId([FromRoute] string pcpId, CancellationToken cancellationToken) => 
        await _mediator.Send(new GetCaseIdByPcpId.GetCaseIdByPcpIdRequest(pcpId), cancellationToken);
}

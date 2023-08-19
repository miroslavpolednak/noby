using System.Net.Mime;
using NOBY.Api.Endpoints.Product.GetProductObligationList.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Product;

[ApiController]
[Route("api/product")]
public sealed class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Vrátí seznam dlužníků a spoludlužníků na daném produktu
    /// </summary>
    /// <remarks>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=DFD7FC8B-6D32-4a81-9608-A502E6F0E74B"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <returns>Seznam klientů na produktu</returns>
    [HttpGet("{caseId:long}/customers")]
    [AuthorizeCaseOwner]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Produkt" })]
    [ProducesResponseType(typeof(List<GetCustomersOnProduct.GetCustomersOnProductCustomer>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<GetCustomersOnProduct.GetCustomersOnProductCustomer>> GetCustomersOnProduct([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCustomersOnProduct.GetCustomersOnProductRequest(caseId), cancellationToken);

    /// <summary>
    /// Vrátí seznam závazků na daném produktu
    /// </summary>
    /// <remarks>
    ///Seznam z závazků načtený z KonsDB.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=058D1B76-375C-4747-90DB-D4E3BE5C46F7"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <returns>Seznam závazků na produktu</returns>
    [HttpGet("{caseId:long}/obligations")]
    [AuthorizeCaseOwner]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Produkt" })]
    [ProducesResponseType(typeof(List<ProductObligation>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductObligations([FromRoute] long caseId, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(new GetProductObligationList.GetProductObligationListRequest(caseId), cancellationToken);

        return items.Any() ? Ok(items) : NoContent();
    }

    /// <summary>
    /// Vrátí Case ID podle zadaného PCP ID
    /// </summary>
    /// <remarks>
    /// Vyhledá Case ID podle PCP ID v KonsDB.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=951AAF15-BEBB-4e22-8DF4-E9C195F309AC"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="pcpId">PCP ID</param>
    [HttpGet("caseid-by-pcpid/{pcpId:required}")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = new[] { "Produkt" })]
    [ProducesResponseType(typeof(GetCaseIdByPcpId.GetCaseIdByPcpIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetCaseIdByPcpId.GetCaseIdByPcpIdResponse> GetCaseIdByPcpId([FromRoute] string pcpId, CancellationToken cancellationToken) => 
        await _mediator.Send(new GetCaseIdByPcpId.GetCaseIdByPcpIdRequest(pcpId), cancellationToken);
}

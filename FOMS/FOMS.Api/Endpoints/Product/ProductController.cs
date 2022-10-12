using FOMS.Api.Endpoints.Product.GetProductObligationList;
using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.Product;

[ApiController]
[Route("api/product")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Vrátí seznam dlužníků a spoludlužníků na daném produktu
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> ProductService/GetCustomersOnProduct<br />
    /// <i>DS:</i> CustomerService/GetList - použito KB ID
    /// </remarks>
    /// <returns>Seznam klientů na produktu</returns>
    [HttpGet("{caseId:long}/customers")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: SalesArrangement" })]
    [ProducesResponseType(typeof(List<GetCustomersOnProduct.GetCustomersOnProductCustomer>), StatusCodes.Status200OK)]
    public async Task<List<GetCustomersOnProduct.GetCustomersOnProductCustomer>> GetCustomersOnProduct([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCustomersOnProduct.GetCustomersOnProductRequest(caseId), cancellationToken);

    /// <summary>
    /// todo:
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="cancellationToken"></param>
    [HttpGet("{productId:long}/obligations")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: SalesArrangement" })]
    [ProducesResponseType(typeof(List<>), StatusCodes.Status200OK)]
    public async Task GetProductObligations([FromRoute] long productId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetProductObligationListRequest(productId), cancellationToken);
}

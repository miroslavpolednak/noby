using NOBY.Api.Endpoints.Product.GetProductObligationList.Dto;
using Swashbuckle.AspNetCore.Annotations;
using GetProductObligationListRequest = NOBY.Api.Endpoints.Product.GetProductObligationList.GetProductObligationListRequest;

namespace NOBY.Api.Endpoints.Product;

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
    [SwaggerOperation(Tags = new[] { "Produkt" })]
    [ProducesResponseType(typeof(List<GetCustomersOnProduct.GetCustomersOnProductCustomer>), StatusCodes.Status200OK)]
    public async Task<List<GetCustomersOnProduct.GetCustomersOnProductCustomer>> GetCustomersOnProduct([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCustomersOnProduct.GetCustomersOnProductRequest(caseId), cancellationToken);

    /// <summary>
    /// Vrátí seznam závazků na daném produktu
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> ProductService/GetProductObligationList<br />
    /// </remarks>
    /// <returns>Seznam závazků na produktu</returns>
    [HttpGet("{caseId:long}/obligations")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Produkt" })]
    [ProducesResponseType(typeof(List<ProductObligation>), StatusCodes.Status200OK)]
    public async Task<List<ProductObligation>> GetProductObligations([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetProductObligationList.GetProductObligationListRequest(caseId), cancellationToken);
}

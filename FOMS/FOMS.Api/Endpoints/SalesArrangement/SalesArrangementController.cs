using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.SalesArrangement;

[ApiController]
[Route("api/sales-arrangement")]
public class SalesArrangementController : ControllerBase
{
    private readonly IMediator _mediator;
    public SalesArrangementController(IMediator mediator) =>  _mediator = mediator;
    
    /// <summary>
    /// Seznam Sales Arrangements pro Case.
    /// </summary>
    /// <remarks>
    /// DS: SalesArrangementService/GetSalesArrangementList
    /// </remarks>
    /// <param name="caseId">ID Case-u</param>
    /// <returns>Seznam zakladnich informaci o vsech Sales Arrangements pro dany Case.</returns>
    [HttpGet("list/{caseId:long}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Case Detail" })]
    [ProducesResponseType(typeof(List<Dto.SalesArrangementListItem>), StatusCodes.Status200OK)]
    public async Task<List<Dto.SalesArrangementListItem>> GetList([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetList.GetListRequest(caseId), cancellationToken);
    
    /// <summary>
    /// Seznam klientu navazanych na Sales Arrangement.
    /// </summary>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    /// <returns>Seznam klientu - aktualne je namockovany pouze jeden staticky klient.</returns>
    [HttpGet("{salesArrangementId:int}/customers")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Case Detail" })]
    [ProducesResponseType(typeof(List<Dto.CustomerListItem>), StatusCodes.Status200OK)]
    public async Task<List<Dto.CustomerListItem>> GetCustomers([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCustomers.GetCustomersRequest(salesArrangementId), cancellationToken);
    
    /// <summary>
    /// Detail Sales Arrangement-u.
    /// </summary>
    /// <remarks>
    /// Obsahuje kompilaci udaju z SA a navazena Offer. Pro kazdy typ produktu se vraci jina struktura Data objektu.
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    /// <returns></returns>
    [HttpGet("{salesArrangementId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Case Detail" })]
    [ProducesResponseType(typeof(GetDetail.GetDetailResponse), StatusCodes.Status200OK)]
    public async Task<GetDetail.GetDetailResponse> GetDetail([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetDetail.GetDetailRequest(salesArrangementId), cancellationToken);
}
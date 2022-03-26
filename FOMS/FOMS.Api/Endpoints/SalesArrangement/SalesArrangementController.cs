using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.SalesArrangement;

[ApiController]
[Route("api/sales-arrangement")]
public class SalesArrangementController : ControllerBase
{
    /// <summary>
    /// Seznam Sales Arrangements pro Case.
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/GetSalesArrangementList
    /// </remarks>
    /// <param name="caseId">ID Case-u</param>
    /// <returns><see cref="List{T}"/> where T : <see cref="Dto.SalesArrangementListItem"/> Seznam zakladnich informaci o vsech Sales Arrangements pro dany Case.</returns>
    [HttpGet("list/{caseId:long}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Case Detail" })]
    [ProducesResponseType(typeof(List<Dto.SalesArrangementListItem>), StatusCodes.Status200OK)]
    public async Task<List<Dto.SalesArrangementListItem>> GetList([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetList.GetListRequest(caseId), cancellationToken);

    /// <summary>
    /// Seznam klientu navazanych na Sales Arrangement.
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> SalesArrangementService/GetCustomerList<br/>
    /// <i>DS:</i> CustomerService/GetCustomerDetail
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    /// <returns><see cref="List{T}"/> where T : <see cref="Dto.CustomerListItem"/> Seznam klientu vc. vsech jejich dat dotazenych z CM atd.</returns>
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
    /// <i>/Endpoints/SalesArrangement/GetDetail</i><br/>
    /// Obsahuje kompilaci udaju z SA a navazena Offer. Pro kazdy typ produktu se vraci jina struktura Data objektu.
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    /// <returns><see cref="GetDetail.GetDetailResponse"/> detail SA vcetne dalsich parametru uveru</returns>
    [HttpGet("{salesArrangementId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Case Detail" })]
    [ProducesResponseType(typeof(GetDetail.GetDetailResponse), StatusCodes.Status200OK)]
    public async Task<GetDetail.GetDetailResponse> GetDetail([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetDetail.GetDetailRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Update ostatnich parametru produktu / uveru.
    /// </summary>
    [HttpPut("{salesArrangementId:int}/parameters")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Ostatni parametry" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task UpdateParameters([FromRoute] int salesArrangementId, [FromBody] UpdateParameters.UpdateParametersRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request.InfuseId(salesArrangementId), cancellationToken);

    /// <summary>
    /// Odeslani SA do SB
    /// </summary>
    [HttpPut("{salesArrangementId:int}/send")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: SendToCmp" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task SendToCmp([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new SendToCmp.SendToCmpRequest(salesArrangementId), cancellationToken);

    private readonly IMediator _mediator;
    public SalesArrangementController(IMediator mediator) =>  _mediator = mediator;
}
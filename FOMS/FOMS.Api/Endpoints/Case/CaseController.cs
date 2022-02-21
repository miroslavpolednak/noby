using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.Case;

[ApiController]
[Route("api/case")]
public class CaseController : ControllerBase
{
    private readonly IMediator _mediator;
    public CaseController(IMediator mediator) =>  _mediator = mediator;
    
    /// <summary>
    /// Detail Case-u.
    /// </summary>
    /// <remarks>
    /// DS: CaseService/GetCaseDetail
    /// </remarks>
    /// <param name="caseId">ID Case-u</param>
    /// <returns>Zakladni informace o Case-u.</returns>
    [HttpGet("{caseId:long}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Case Detail" })]
    [ProducesResponseType(typeof(Dto.CaseModel), StatusCodes.Status200OK)]
    public async Task<Dto.CaseModel> GetById([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetById.GetByIdRequest(caseId), cancellationToken);
    
    /// <summary>
    /// Pocty Cases pro prihlaseneho uzivatele zgrupovane podle stavu.
    /// </summary>
    /// <remarks>
    /// DS: CseService/GetCaseCounts
    /// </remarks>
    /// <returns>Kolekce ID stavu s poctem Cases.</returns>
    [HttpGet("totals-by-states")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Dashboard" })]
    [ProducesResponseType(typeof(List<GetTotalsByStates.GetTotalsByStatesResponse>), StatusCodes.Status200OK)]
    public async Task<List<GetTotalsByStates.GetTotalsByStatesResponse>> GetTotalsByStates(CancellationToken cancellationToken)
        => await _mediator.Send(new GetTotalsByStates.GetTotalsByStatesRequest(), cancellationToken);
    
    /// <summary>
    /// Seznam Cases pro prihlaseneho uzivatele.
    /// </summary>
    /// <remarks>
    /// Endpoint umoznuje:
    /// - vyhledat Case podle retezce
    /// - zobrazit pouze Cases v pozadovanem stavu
    /// - nastavit strankovani
    /// - nastavit razeni [stateUpdated]
    /// DS: CaseService/SearchCases
    /// </remarks>
    /// <param name="request">Nastaveni moznosti filtrovani, strankovani a razeni.</param>
    /// <returns>Seznam Cases + informace o pouzitem strankovani/razeni.</returns>
    [HttpGet("search")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Dashboard" })]
    [ProducesResponseType(typeof(Search.SearchResponse), StatusCodes.Status200OK)]
    public async Task<Search.SearchResponse> Search([FromBody] Search.SearchRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
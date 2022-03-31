using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.Cases;

[ApiController]
[Route("api/case")]
public class CasesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CasesController(IMediator mediator) =>  _mediator = mediator;
    
    /// <summary>
    /// Detail Case-u.
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> CaseService/GetCaseDetail<br/>
    /// https://wiki.kb.cz/confluence/display/HT/getCaseDetail
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
    /// <i>DS:</i> CseService/GetCaseCounts<br/>
    /// https://wiki.kb.cz/confluence/display/HT/getCaseCounts
    /// </remarks>
    /// <returns>Kolekce ID stavu s poctem Cases.</returns>
    [HttpGet("totals-by-states")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Dashboard" })]
    [ProducesResponseType(typeof(List<GetTotalsByStates.GetDashboardFiltersResponse>), StatusCodes.Status200OK)]
    public async Task<List<GetTotalsByStates.GetDashboardFiltersResponse>> GetDashboardFilters(CancellationToken cancellationToken)
        => await _mediator.Send(new GetTotalsByStates.GetDashboardFiltersRequest(), cancellationToken);
    
    /// <summary>
    /// Seznam Cases pro prihlaseneho uzivatele.
    /// </summary>
    /// <remarks>
    /// Endpoint umoznuje:
    /// - vyhledat Case podle retezce
    /// - zobrazit pouze Cases v pozadovanem stavu
    /// - nastavit strankovani
    /// - nastavit razeni [stateUpdated]
    /// <i>DS:</i> CaseService/SearchCases<br/>
    /// https://wiki.kb.cz/confluence/display/HT/searchCases
    /// </remarks>
    /// <param name="request">Nastaveni moznosti filtrovani, strankovani a razeni.</param>
    /// <returns>Seznam Cases + informace o pouzitem strankovani/razeni.</returns>
    [HttpPost("search")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Dashboard" })]
    [ProducesResponseType(typeof(Search.SearchResponse), StatusCodes.Status200OK)]
    public async Task<Search.SearchResponse> Search([FromBody] Search.SearchRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
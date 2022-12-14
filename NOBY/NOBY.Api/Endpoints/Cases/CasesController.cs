using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Cases;

[ApiController]
[Route("api/case")]
public class CasesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CasesController(IMediator mediator) =>  _mediator = mediator;

    /// <summary>
    /// Vytvoření servisního SalesArrangement-u
    /// </summary>
    /// <remarks>
    /// Vytvoří nový servisní sales arrangement dle zadaného typu.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=B3D75222-1F0D-4dc6-A228-BD237F42CA44"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20"/>Diagram v EA</a><br /><br />
    /// Jako error handling vrací FE API textaci chyby pro zobrazení  na FE přímo v title.<br/><br/>
    /// Pokud typ žádosti je žádost o čerpání (SalesArrangementTypeId = 6) dochází k replikaci čísla účtu pro splácení a nastavování příznaku IsAccountNumberMissing podle toho, jestli při vytváření sales arrangementu číslo účtu v KonsDB existuje.
    /// </remarks>
    [HttpPost("{caseId:long}/sales-arrangement")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Case" })]
    [ProducesResponseType(typeof(CreateSalesArrangement.CreateSalesArrangementResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<CreateSalesArrangement.CreateSalesArrangementResponse> CreateSalesArrangement([FromRoute] long caseId, [FromBody] CreateSalesArrangement.CreateSalesArrangementRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request.InfuseId(caseId), cancellationToken);

    /// <summary>
    /// Detail dlužníků a spoludlužníků pro daný case
    /// </summary>
    /// <remarks>
    /// Vrací se seznam detailů customerů - filtruje se  na dlužníky a spoludlužníky.<br />
    /// Dlužník na prvním místě, dále spoludlužníci řazeni dle příjmení a jména.<br /><br />
    /// <i>DS:</i> SalesArrangementService/getSalesArrangementList<br /><i>DS:</i> SalesArrangementService/GetCustomerList (onSA)<br />
    /// <i>DS:</i> ProductService/GetCustomersOnProduct<br /><i>DS:</i> CustomerService/GetList<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=7F566103-49D8-4a7a-83C3-B690F4A1CC1C"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="caseId">ID Case-u</param>
    [HttpGet("{caseId:long}/customers")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Case" })]
    [ProducesResponseType(typeof(List<GetCustomers.GetCustomersResponseCustomer>), StatusCodes.Status200OK)]
    public async Task<List<GetCustomers.GetCustomersResponseCustomer>> GetCustomers([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCustomers.GetCustomersRequest(caseId), cancellationToken);

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
    [SwaggerOperation(Tags = new [] { "Case" })]
    [ProducesResponseType(typeof(Dto.CaseModel), StatusCodes.Status200OK)]
    public async Task<Dto.CaseModel> GetById([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetById.GetByIdRequest(caseId), cancellationToken);
    
    /// <summary>
    /// Pocty Cases pro prihlaseneho uzivatele zgrupovane podle nastavenych filtru.
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> CseService/GetCaseCounts<br/>
    /// https://wiki.kb.cz/confluence/display/HT/getCaseCounts
    /// </remarks>
    /// <returns>Kolekce ID stavu s poctem Cases.</returns>
    [HttpGet("dashboard-filters")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "Case" })]
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
    /// - nastavit razeni [povolene: stateUpdated, customerName]
    /// <i>DS:</i> CaseService/SearchCases<br/>
    /// https://wiki.kb.cz/confluence/display/HT/searchCases
    /// </remarks>
    /// <param name="request">Nastaveni moznosti filtrovani, strankovani a razeni.</param>
    /// <returns>Seznam Cases + informace o pouzitem strankovani/razeni.</returns>
    [HttpPost("search")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new [] { "Case" })]
    [ProducesResponseType(typeof(Search.SearchResponse), StatusCodes.Status200OK)]
    public async Task<Search.SearchResponse> Search([FromBody] Search.SearchRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);

    /// <summary>
    /// Seznam workflow tasku dotazeny z SB.
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> CaseService/GetTaskList<br/>
    /// </remarks>
    /// <returns>Seznam wf tasks z SB.</returns>
    [HttpGet("{caseId:long}/tasks")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Case" })]
    [ProducesResponseType(typeof(GetTaskList.GetTaskListResponse), StatusCodes.Status200OK)]
    public async Task<GetTaskList.GetTaskListResponse> GetTaskList([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetTaskList.GetTaskListRequest(caseId), cancellationToken);

    /// <summary>
    /// Parametry Case-u.
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> CaseService/GetTaskList<br/>
    /// </remarks>
    /// <returns>Parametry Case-u (Hodnoty parametrů se načítají z různých zdrojů dle stavu Case).</returns>
    [HttpGet("{caseId:long}/parameters")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Case" })]
    [ProducesResponseType(typeof(GetCaseParameters.GetCaseParametersResponse), StatusCodes.Status200OK)]
    public async Task<GetCaseParameters.GetCaseParametersResponse> GetCaseParameters([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCaseParameters.GetCaseParametersRequest(caseId), cancellationToken);
}
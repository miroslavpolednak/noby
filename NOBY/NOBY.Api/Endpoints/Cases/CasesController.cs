﻿using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Cases;

[ApiController]
[Route("api/case")]
public class CasesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CasesController(IMediator mediator) =>  _mediator = mediator;

    /// <summary>
    /// Stornování úkolu ve SB
    /// </summary>
    /// <remarks>
    /// Stornování úkolu ve SB. <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=C77A111D-090F-410c-A1B2-B0E4E3EA59CF"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("{caseId:long}/tasks/{taskId:int}/cancel", Name = "taskDetailCancel")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Case" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelTask([FromRoute] long caseId, [FromRoute] int taskId, [FromBody] CancelTask.CancelTaskRequest request)
    {
        await _mediator.Send(request.InfuseId(caseId, taskId));
        return NoContent();
    }
    
    /// <summary>
    /// Vytvoření nového workflow tasku do SB.
    /// </summary>
    /// <remarks>
    /// Vytvoření nového workflow tasku do SB.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=882E85E3-F6EA-4774-812E-3328006E8893"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <returns>Noby task ID. Jde o ID sady úkolů generované Starbuildem.</returns>
    [HttpPost("{caseId:long}/tasks", Name = "taskCreate")]
    [Consumes("application/json")]
    [Produces("text/plain")]
    [SwaggerOperation(Tags = new[] { "Case" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<int> CreateTask([FromRoute] long caseId, [FromBody] CreateTask.CreateTaskRequest request)
        => await _mediator.Send(request.InfuseId(caseId));

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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<CreateSalesArrangement.CreateSalesArrangementResponse> CreateSalesArrangement([FromRoute] long caseId, [FromBody] CreateSalesArrangement.CreateSalesArrangementRequest request)
        => await _mediator.Send(request.InfuseId(caseId));

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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Dto.CaseModel> GetById([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetById.GetByIdRequest(caseId), cancellationToken);
    
    /// <summary>
    /// Počty Cases pro přihlášeného uživatele zgrupované podle nastavených filtrů.
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> CseService/GetCaseCounts<br/>
    /// https://wiki.kb.cz/confluence/display/HT/getCaseCounts
    /// </remarks>
    /// <returns>Kolekce ID stavu s počtem Cases.</returns>
    [HttpGet("dashboard-filters")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "Case" })]
    [ProducesResponseType(typeof(List<GetTotalsByStates.GetDashboardFiltersResponse>), StatusCodes.Status200OK)]
    public async Task<List<GetTotalsByStates.GetDashboardFiltersResponse>> GetDashboardFilters(CancellationToken cancellationToken)
        => await _mediator.Send(new GetTotalsByStates.GetDashboardFiltersRequest(), cancellationToken);

    /// <summary>
    /// Seznam Cases pro přihlášeného uživatele.
    /// </summary>
    /// <remarks>
    /// Endpoint umožnuje:
    /// - vyhledat Case podle řetězce
    /// - zobrazit pouze Cases v požadovaném stavu
    /// - nastavit stránkovaní
    /// - nastavit řazení [povolené: stateUpdated, customerName]
    /// <i>DS:</i> CaseService/SearchCases<br/>
    /// https://wiki.kb.cz/confluence/display/HT/searchCases
    /// </remarks>
    /// <param name="request">Nastavení možnosti filtrovaní, strankovaní a řazení.</param>
    /// <returns>Seznam Cases + informace o použitém stránkovaní/řazení.</returns>
    [HttpPost("search")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new [] { "Case" })]
    [ProducesResponseType(typeof(Search.SearchResponse), StatusCodes.Status200OK)]
    public async Task<Search.SearchResponse> Search([FromBody] Search.SearchRequest request)
        => await _mediator.Send(request);

    /// <summary>
    /// Seznam workflow tasku dotažený z SB.
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> CaseService/GetTaskList<br/>
    /// </remarks>
    /// <returns>Seznam wf tasks z SB.</returns>
    [HttpGet("{caseId:long}/tasks")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Case" })]
    [ProducesResponseType(typeof(GetTaskList.GetTaskListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetTaskList.GetTaskListResponse> GetTaskList([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetTaskList.GetTaskListRequest(caseId), cancellationToken);

    /// <summary>
    /// Parametry Case-u.
    /// </summary>
    /// <remarks>
    /// Vrátí parametry case s ohledem na stav case. Před předáním žádosti vrací lokálně uložená data. Po předání žádosti vrací data z KonsDB. <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=EEB4BBAA-9996-43ff-BB50-61514A2B6107"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <returns>Parametry Case-u (Hodnoty parametrů se načítají z různých zdrojů dle stavu Case).</returns>
    [HttpGet("{caseId:long}/parameters")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Case" })]
    [ProducesResponseType(typeof(GetCaseParameters.GetCaseParametersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetCaseParameters.GetCaseParametersResponse> GetCaseParameters([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCaseParameters.GetCaseParametersRequest(caseId), cancellationToken);
}
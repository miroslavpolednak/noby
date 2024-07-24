using Asp.Versioning;
using NOBY.Api.Endpoints.Cases.GetCaseDocumentsFlag;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Cases;

[ApiController]
[Route("api/v{v:apiVersion}/case")]
[ApiVersion(1)]
public class CasesController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Stornování case-u
    /// </summary>
    /// <remarks>
    /// Stornování case-u. Slouží k stornování obchodního případu, který je ještě ve fázi žádosti v NOBY.
    /// </remarks>
    [HttpPost("{caseId:long}/cancel")]
    [NobyAuthorize(UserPermissions.CASE_Cancel, UserPermissions.SALES_ARRANGEMENT_Access)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = [ "Case" ])]
    [ProducesResponseType(typeof(CasesCancelCaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=B13A9B30-5896-4319-A96E-0982FE5A9045")]
    public async Task<CasesCancelCaseResponse> CancelCase([FromRoute] long caseId)
        => await _mediator.Send(new CancelCase.CancelCaseRequest(caseId));

    /// <summary>
    /// Detail dlužníků a spoludlužníků pro daný case
    /// </summary>
    /// <remarks>
    /// Vrací se seznam detailů customerů - filtruje se  na dlužníky a spoludlužníky.<br />
    /// Dlužník na prvním místě, dále spoludlužníci řazeni dle příjmení a jména.<br /><br />
    /// <i>DS:</i> SalesArrangementService/getSalesArrangementList<br /><i>DS:</i> SalesArrangementService/GetCustomerList (onSA)<br />
    /// <i>DS:</i> ProductService/GetCustomersOnProduct<br /><i>DS:</i> CustomerService/GetList
    /// </remarks>
    /// <param name="caseId">ID Case-u</param>
    [HttpGet("{caseId:long}/customers")]
    [NobyAuthorizePreload(NobyAuthorizePreloadAttribute.LoadableEntities.Case)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = [ "Case" ])]
    [ProducesResponseType(typeof(List<CasesGetCustomersResponseCustomer>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=7F566103-49D8-4a7a-83C3-B690F4A1CC1C")]
    public async Task<List<CasesGetCustomersResponseCustomer>> GetCustomersOnCase([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCustomersOnCase.GetCustomersOnCaseRequest(caseId), cancellationToken);

    /// <summary>
    /// Vytvoření servisního SalesArrangement-u
    /// </summary>
    /// <remarks>
    /// Vytvoří nový servisní sales arrangement dle zadaného typu.
    /// 
    /// Jako error handling vrací FE API textaci chyby pro zobrazení  na FE přímo v title.<br/><br/>
    /// Pokud typ žádosti je žádost o čerpání (SalesArrangementTypeId = 6) dochází k replikaci čísla účtu pro splácení a nastavování příznaku IsAccountNumberMissing podle toho, jestli při vytváření sales arrangementu číslo účtu v KonsDB existuje.
    /// </remarks>
    [HttpPost("{caseId:long}/sales-arrangement")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(typeof(SalesArrangementCreateSalesArrangementResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=B3D75222-1F0D-4dc6-A228-BD237F42CA44")]
    public async Task<SalesArrangementCreateSalesArrangementResponse> CreateSalesArrangement([FromRoute] long caseId, [FromBody] SalesArrangementCreateSalesArrangementRequest request)
        => await _mediator.Send(request.InfuseId(caseId));

    /// <summary>
    /// Detail Case-u.
    /// </summary>
    /// <remarks>
    /// Načtení detailu Case-u
    /// </remarks>
    /// <param name="caseId">ID Case-u</param>
    /// <returns>Zakladni informace o Case-u.</returns>
    [HttpGet("{caseId:long}")]
    [NobySkipCaseOwnerValidation]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = [ "Case" ])]
    [ProducesResponseType(typeof(CasesSharedCaseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=CCF1229F-2E77-4de4-8E4A-665594BCD9CA")]
    public async Task<CasesSharedCaseModel> GetCaseById([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCaseById.GetCaseByIdRequest(caseId), cancellationToken);

    /// <summary>
    /// Počty cases pro dashboard
    /// </summary>
    /// <remarks>
    /// Počty Cases pro přihlášeného uživatele zgrupované podle nastavených filtrů.
    /// </remarks>
    [HttpGet("dashboard-filters")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = [ "Case" ])]
    [ProducesResponseType(typeof(List<CasesGetDashboardFiltersResponseItem>), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=2FDB0893-BE64-4f37-A196-DDECBB910CB3")]
    public async Task<List<CasesGetDashboardFiltersResponseItem>> GetDashboardFilters(CancellationToken cancellationToken)
        => await _mediator.Send(new GetDashboardFilters.GetDashboardFiltersRequest(), cancellationToken);

    /// <summary>
    /// Seznam Cases pro přihlášeného uživatele.
    /// </summary>
    /// <remarks>
    /// Endpoint umožnuje:<br />
    /// - vyhledat Case podle řetězce<br />
    /// - zobrazit pouze Cases v požadovaném stavu<br />
    /// - nastavit stránkovaní<br />
    /// - nastavit řazení [povolené: stateUpdated, customerName]
    /// </remarks>
    /// <param name="request">Nastavení možnosti filtrovaní, strankovaní a řazení.</param>
    /// <returns>Seznam Cases + informace o použitém stránkovaní/řazení.</returns>
    [HttpPost("search")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.DASHBOARD_SearchCases)]
    [SwaggerOperation(Tags = [ "Case" ])]
    [ProducesResponseType(typeof(CasesSearchCasesResponse), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=62A33551-BC77-401d-80DB-E8DFA5081719")]
    public async Task<CasesSearchCasesResponse> SearchCases([FromBody] CasesSearchCasesRequest request)
        => await _mediator.Send(request);

    /// <summary>
    /// Identifikace obchodních případy podle jednoho níže uvedených možných kritérií.
    /// </summary>
    /// <remarks>
    /// Endpoint umožnuje identifikovat obchodní případy podle:
    /// 
    /// - čárového kódu dokumentu (formId) (vždy 1 case)
    /// - čísla úvěrového účtu (vždy 1 case)
    /// - ID obchodního případu (vždy 1 case)
    /// - čísla smlouvy (vždy 1 case)
    /// - identity klienta (vrací kolekci case-s)
    /// </remarks>
    /// <param name="request">Typ kritéria a jeho hodnota pro vyhledávání.</param>
    [HttpPost("identify")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobySkipCaseOwnerValidation]
    [NobyAuthorize(UserPermissions.DASHBOARD_IdentifyCase)]
    [SwaggerOperation(Tags = [ "Case" ])]
    [ProducesResponseType(typeof(CasesIdentifyCaseResponse), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=60DB6DA4-D938-4901-98DC-0C8DF8011589")]
    public async Task<CasesIdentifyCaseResponse> IdentifyCase([FromBody] CasesIdentifyCaseRequest request)
        => await _mediator.Send(request);
    
    /// <summary>
    /// Parametry Case-u.
    /// </summary>
    /// <remarks>
    /// Vrátí parametry case s ohledem na stav case. Před předáním žádosti vrací lokálně uložená data. Po předání žádosti vrací data z KonsDB.
    /// </remarks>
    /// <returns>Parametry Case-u (Hodnoty parametrů se načítají z různých zdrojů dle stavu Case).</returns>
    [HttpGet("{caseId:long}/parameters")]
    [NobyAuthorizePreload(NobyAuthorizePreloadAttribute.LoadableEntities.Case)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = [ "Case" ])]
    [ProducesResponseType(typeof(CasesGetCaseParametersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=EEB4BBAA-9996-43ff-BB50-61514A2B6107")]
    public async Task<CasesGetCaseParametersResponse> GetCaseParameters([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCaseParameters.GetCaseParametersRequest(caseId), cancellationToken);


    /// <summary>
    /// Příznaky v menu Case detailu
    /// </summary>
    /// <remarks>
    ///  Implementováno pro položku menu Documents
    /// </remarks>
    /// <param name="caseId">ID Case-u</param>
    [HttpGet("{caseId:long}/menu/flags")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = [ "Case" ])]
    [ProducesResponseType(typeof(CasesGetCaseMenuFlagsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=2E9DAC5D-A7F3-49a4-804D-770418854A10")]
    public async Task<CasesGetCaseMenuFlagsResponse> GetCaseMenuFlags([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCaseMenuFlagsRequest(caseId), cancellationToken);

    /// <summary>
    /// Podmínky ke splnění Case-u
    /// </summary>
    /// <remarks>
    /// Seznam Podmínek ke splnění pro Case
    /// </remarks>
    /// <param name="caseId">ID Case-u</param>
    [HttpGet("{caseId:long}/covenants")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = [ "Case" ])]
    [ProducesResponseType(typeof(CasesGetCovenantsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=4378EDFB-2A3D-46f6-8C51-0241A3436D5E")]
    public async Task<CasesGetCovenantsResponse> GetCovenants([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCovenants.GetCovenantsRequest(caseId), cancellationToken);

    /// <summary>
    /// Detail Podmínky ke splnění Case-u
    /// </summary>
    /// <remarks>
    /// Detail Podmínky ke splnění pro Case
    /// </remarks>
    /// <param name="caseId">ID Case-u</param>
    /// <param name="covenantOrder">Pořadí podmínky ke splnění</param>
    [HttpGet("{caseId:long}/covenant/{covenantOrder:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = [ "Case" ])]
    [ProducesResponseType(typeof(CasesGetCovenantResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=56FAFD66-E483-475d-9868-6B90A4ED889B")]
    public async Task<CasesGetCovenantResponse> GetCovenant([FromRoute] long caseId, [FromRoute] int covenantOrder, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCovenant.GetCovenantRequest(caseId, covenantOrder), cancellationToken);
}
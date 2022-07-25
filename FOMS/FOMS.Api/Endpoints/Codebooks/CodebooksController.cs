using DomainServices.CodebookService.Abstraction;
using DomainServices.CodebookService.Contracts.Endpoints.LoanKinds;

namespace FOMS.Api.Endpoints.Codebooks;

[ApiController]
[Route("api/codebooks")]
public class CodebooksController : ControllerBase
{
    private readonly IMediator _mediator;
    public CodebooksController(IMediator mediator) =>  _mediator = mediator;

    /// <summary>Kolekce vyzadanych ciselniku.</summary>
    /// <remarks>
    /// Vraci ciselniky identifikovane query parametrem "q". Jednotlive ciselniky jsou oddelene carkou.<br/>
    /// Aktualne implementovane ciselniky jsou:
    /// - <a href="https://wiki.kb.cz/confluence/pages/viewpage.action?pageId=421388420">AcademicDegreesAfter</a>
    /// - <a href="https://wiki.kb.cz/confluence/pages/viewpage.action?pageId=421388414">AcademicDegreesBefore</a>
    /// - ActionCodesSavings...
    /// - ActionCodesSavingsLoan
    /// - <a href="https://wiki.kb.cz/confluence/display/HT/CaseState">CaseStates</a>
    /// - <a href="https://wiki.kb.cz/confluence/pages/viewpage.action?pageId=430216251">ClassificationOfEconomicActivities</a>
    /// - <a href="https://wiki.kb.cz/confluence/pages/viewpage.action?pageId=392877756">ContactTypes</a>
    /// - <a href="https://wiki.kb.cz/confluence/pages/viewpage.action?pageId=405522415">Countries</a>
    /// - <a href="https://wiki.kb.cz/confluence/pages/viewpage.action?pageId=424127746">Currencies</a>
    /// - <a href="https://wiki.kb.cz/confluence/pages/viewpage.action?pageId=421386916">CustomerRoles</a>
    /// - BankCodes
    /// - Developers
    /// - DeveloperProjects
    /// - DrawingDurations
    /// - EducationLevels
    /// - EmploymentTypes
    /// - Fees
    /// - FixedRatePeriods
    /// - Genders
    /// - HouseholdTypes
    /// - IdentificationDocumentTypes
    /// - IncomeMainTypes
    /// - IncomeForeignTypes
    /// - IncomeOtherTypes
    /// - JobTypes
    /// - LegalCapacities
    /// - LoanPurposes
    /// - LoanKinds
    /// - Mandants
    /// - MaritalStatuses
    /// - ObligationTypes
    /// - PaymentDays
    /// - PostCodes
    /// - ProductTypes
    /// - PropertySettlements
    /// - RealEstateTypes
    /// - RealEstatePurchaseTypes
    /// - SalesArrangementStates
    /// - SalesArrangementTypes
    /// - SignatureTypes
    /// - WorkflowTaskCategories
    /// - WorkflowTaskStates
    /// - WorkflowTaskTypes
    /// - WorkSectors
    /// </remarks>
    /// <param name="codebookTypes">Kody pozadovanych ciselniku oddelene carkou. Nazvy NEjsou case-sensitive. Example: q=productTypes,actionCodesSavings</param>
    /// <returns>Kolekce vyzadanych ciselniku.</returns>
    [HttpGet("get-all")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<GetAll.GetAllResponseItem>), StatusCodes.Status200OK)]
    public async Task<List<GetAll.GetAllResponseItem>> GetAll([FromQuery(Name = "q")] string codebookTypes, CancellationToken cancellationToken)
        => await _mediator.Send(new GetAll.GetAllRequest(codebookTypes), cancellationToken);
    
    /// <summary>
    /// Ciselnik fixace uveru.
    /// </summary>
    /// <returns>Kolekce dob fixaci v mesicich.</returns>
    /// <param name="productTypeId">ID typu produktu, pro ktery se maji vratit fixace.</param>
    [HttpGet("fixation-period-length")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<int>), StatusCodes.Status200OK)]
    public async Task<List<int>> GetFixationPeriodLength([FromQuery] int productTypeId, [FromServices] ICodebookServiceAbstraction svc, CancellationToken cancellationToken)
        => (await svc.FixedRatePeriods(cancellationToken))
            .Where(t => t.ProductTypeId == productTypeId)
            .Select(t => t.FixedRatePeriod)
            .OrderBy(t => t)
            .ToList();

    /// <summary>
    /// Ciselnik druhu uveru.
    /// </summary>
    /// <param name="productTypeId">ID typu produktu, pro ktery se maji vratit druhy uveru.</param>
    [HttpGet("product-loan-kinds")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<LoanKindsItem>), StatusCodes.Status200OK)]
    public async Task<List<LoanKindsItem>?> GetProductLoanKinds([FromQuery] int productTypeId, [FromServices] ICodebookServiceAbstraction svc, CancellationToken cancellationToken)
    {
        var loanKindsIds = (await svc.ProductTypes(cancellationToken))
            .FirstOrDefault(t => t.Id == productTypeId && t.IsValid)?
            .LoanKindIds
            .ToList();
        if (loanKindsIds is null) return null;

        return (await svc.LoanKinds(cancellationToken))
            .Where(t => loanKindsIds.Contains(t.Id))
            .ToList();
    }

    /// <summary>
    /// FixedRatePeriod s filtraci na product
    /// </summary>
    /// <param name="productTypeId">ID typu produktu</param>
    [HttpGet("fixed-rate-periods")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<DomainServices.CodebookService.Contracts.Endpoints.FixedRatePeriods.FixedRatePeriodsItem>), StatusCodes.Status200OK)]
    public async Task<List<DomainServices.CodebookService.Contracts.Endpoints.FixedRatePeriods.FixedRatePeriodsItem>?> GetFixedRatePeriods([FromQuery] int productTypeId, [FromServices] ICodebookServiceAbstraction svc, CancellationToken cancellationToken)
        => (await svc.FixedRatePeriods(cancellationToken))
            .Where(t => t.ProductTypeId == productTypeId && t.IsNewProduct && t.IsValid)
            .DistinctBy(t => new { t.FixedRatePeriod, t.MandantId })
            .ToList();

    /// <summary>
    /// Vyhledání developerských projektů
    /// </summary>
    /// <remarks>
    /// Vyhledá developerské projekty na základě vyhledávacího textu.<br />
    /// Vyhledává se v číselníku <a href="https://wiki.kb.cz/confluence/pages/viewpage.action?pageId=438046695">Developer (CIS_DEVELOPER)</a> v atributech Name (NAZEV) a Cin (ICO_RC) a v číselníku <a href="https://wiki.kb.cz/confluence/pages/viewpage.action?pageId=438046776">DeveloperProject (CIS_DEVELOPER_PROJEKTY_SPV)</a> v atributu Name (PROJEKT).<br />
    /// Text se vyhledává jako subřetězce v uvedených sloupcích - ty jsou oddělené ve vyhledávacím textu mezerou.
    /// </remarks>
    [HttpPost("developer-project/search")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DeveloperSearch.DeveloperSearchResponse), StatusCodes.Status200OK)]
    public async Task<DeveloperSearch.DeveloperSearchResponse> DeveloperSearch([FromBody] DeveloperSearch.DeveloperSearchRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
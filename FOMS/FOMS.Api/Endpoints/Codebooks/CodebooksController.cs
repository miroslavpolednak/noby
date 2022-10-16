﻿using DomainServices.CodebookService.Abstraction;
using DomainServices.CodebookService.Contracts.Endpoints.LoanKinds;

namespace FOMS.Api.Endpoints.Codebooks;

[ApiController]
[Route("api/codebooks")]
public class CodebooksController : ControllerBase
{
    private readonly IMediator _mediator;
    public CodebooksController(IMediator mediator) =>  _mediator = mediator;

    /// <summary>Kolekce vyžadaných číselniků.</summary>
    /// <remarks>
    /// Vrací číselníky identifikované query parametrem "q". Jednotlivé číselniky jsou oddělené čárkou.<br/>
    /// Aktuálně implementované číselniky jsou:
    ///
    /// Na stránkách CFL (níže jména číselníků - odkazy na číselníky do CFL) je uvedeno u sloupců <b>(není v FE API)</b>, pokud nejsou na výstupu FE API.
    ///
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=421388420">AcademicDegreesAfter</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=421388414">AcademicDegreesBefore</a>
    /// - ActionCodesSavings - obsolete
    /// - ActionCodesSavingsLoan - obsolete
    /// - BankCodes
    /// - <a href="https://wiki.kb.cz/confluence/display/HT/CaseState">CaseStates</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=430216251">ClassificationOfEconomicActivities</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=392877756">ContactTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=405522415">Countries</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=424127746">Currencies</a>
    /// - <a href="https://wiki.kb.cz/display/HT/CustomerProfile">CustomerProfiles</a> - zatím není na FE API implementováno
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=421386916">CustomerRoles</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=455007953">BankCodes</a> - zatím není na FE API implementováno
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438046695">Developers</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438046776">DeveloperProjects</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=450580207">DrawingDurations</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=444604999">DrawingTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=400823251">EaCodesMain</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=419562802">EducationLevels</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=430216233">EmploymentTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438049777">Fees</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413645701">FixedRatePeriods</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=405524365">Genders</a>
    /// - <a href="https://wiki.kb.cz/display/HT/HouseholdType+-+MOCK">HouseholdTypes</a> - zatím není na FE API implementováno
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=392871926">IdentificationDocumentTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=426158089">IncomeMainTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=430216188">IncomeForeignTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=440881532">IncomeOtherTypes</a> - zatím není na FE API implementováno
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=430216289">JobTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=424118208">LegalCapacities</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413109663">LoanPurposes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413108848">LoanKinds</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413108848">LoanInterestRateAnnouncedTypes</a>
    /// - <a href="https://wiki.kb.cz/display/HT/Mandant">Mandants</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=417279748">MaritalStatuses</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=417279748">MarketingActions</a>  - zatím není na FE API implementováno
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=440890324">ObligationCorrectionTypes</a> - zatím není na FE API implementováno
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=426150084">ObligationTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438046178">PaymentDays</a>
    /// - <a href="https://wiki.kb.cz/display/HT/PayoutType">PayoutTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=421389753">PostCodes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=392883923">ProductTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413648025">PropertySettlements</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413632253">RealEstateTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413632953">RealEstatePurchaseTypes</a>
    /// - <a href="https://wiki.kb.cz/display/HT/SalesArrangementState">SalesArrangementStates</a>
    /// - <a href="https://wiki.kb.cz/display/HT/SalesArrangementType">SalesArrangementTypes</a>
    /// - <a href="https://wiki.kb.cz/display/HT/SignatureType">SignatureTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=417284324">StatementTypes</a>
    /// - <a href="https://wiki.kb.cz/display/HT/WorkflowTaskCategory">WorkflowTaskCategories</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=440871662">WorkflowTaskStates</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=440879561">WorkflowTaskTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=430216261">WorkSectors</a>
    ///
    /// Struktura číselníku který není k nalezení dle jména ve schematech response je schema GenericCodebookItem.
    ///
    /// </remarks>
    /// <param name="codebookTypes">Kody pozadovanych ciselniku oddelene carkou. Nazvy NEjsou case-sensitive. Example: q=productTypes,actionCodesSavings</param>
    /// <returns>Kolekce vyzadanych ciselniku.</returns>
    [HttpGet("get-all")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<GetAll.GetAllResponseItem>), StatusCodes.Status200OK)]
    public async Task<List<GetAll.GetAllResponseItem>> GetAll([FromQuery(Name = "q")] string codebookTypes, CancellationToken cancellationToken)
        => await _mediator.Send(new GetAll.GetAllRequest(codebookTypes), cancellationToken);

    [HttpGet("supported")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<CodebookMap.SupportedCodebook>), StatusCodes.Status200OK)]
    public List<CodebookMap.SupportedCodebook> GetSupported([FromServices] CodebookMap.ICodebookMap codebookMap)
    {
        return codebookMap.Select(m => new CodebookMap.SupportedCodebook
        {
            Name = m.Code,
            Type = m.ReturnType.Name
        }).ToList();
    }

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
    /// Vyhledává se v číselníku <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438046695">Developer (CIS_DEVELOPER)</a> v atributech Name (NAZEV) a Cin (ICO_RC) a v číselníku <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438046776">DeveloperProject (CIS_DEVELOPER_PROJEKTY_SPV)</a> v atributu Name (PROJEKT).<br />
    /// Text se vyhledává jako subřetězce v uvedených sloupcích - ty jsou oddělené ve vyhledávacím textu mezerou.
    /// </remarks>
    [HttpPost("developer-project/search")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DeveloperSearch.DeveloperSearchResponse), StatusCodes.Status200OK)]
    public async Task<DeveloperSearch.DeveloperSearchResponse> DeveloperSearch([FromBody] DeveloperSearch.DeveloperSearchRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
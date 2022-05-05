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
    /// - AcademicDegreesAfter
    /// - AcademicDegreesBefore
    /// - ActionCodesSavings
    /// - ActionCodesSavingsLoan
    /// - CaseStates
    /// - ClassificationOfEconomicActivities
    /// - ContactTypes
    /// - Countries
    /// - Currencies
    /// - CustomerRoles
    /// - EducationLevels
    /// - EmploymentTypes
    /// - FixedRatePeriods
    /// - Genders
    /// - IdentificationDocumentTypes
    /// - IncomeTypes
    /// - IncomeAbroadTypes
    /// - JobTypes
    /// - LegalCapacities
    /// - LoanPurposes
    /// - LoanKinds
    /// - Mandants
    /// - MaritalStatuses
    /// - ObligationTypes
    /// - PostCodes
    /// - ProductTypes
    /// - PropertySettlement
    /// - RealEstateTypes
    /// - RealEstatePurchaseTypes
    /// - SalesArrangementStates
    /// - SalesArrangementTypes
    /// - SignatureTypes
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
        => (await svc.ProductTypes(cancellationToken))
            .FirstOrDefault(t => t.Id == productTypeId)?
            .LoanKinds
            .Where(t => t.IsValid)
            .ToList();
}